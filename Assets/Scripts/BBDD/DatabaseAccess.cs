using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;
using System;

public class DatabaseAccess
{
    MongoClient client = new MongoClient("mongodb+srv://mgalvezdam:miquel123@cluster0.rj5yhrh.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0");
    IMongoDatabase database;
    IMongoCollection<BsonDocument> collection;


    // Saves a player's name and initial score to the database
    public void SaveName(string name)
    {
        double score = 0;
        database = client.GetDatabase("UnityDB");
        collection = database.GetCollection<BsonDocument>("Players");

        // Creates a document with player's name and initial score
        var document = new BsonDocument { { "player", name }, { "score", score } };
        collection.InsertOne(document);
    }

    // Updates a player's score in the database
    public void UpdateScore(string playerName, double scoreToAdd)
    {
        if (collection == null)
        {
            // Initializes the collection if not already initialized
            database = client.GetDatabase("UnityDB");
            collection = database.GetCollection<BsonDocument>("Players");
        }

        // Retrieves the player's document from the database
        var filter = Builders<BsonDocument>.Filter.Eq("player", playerName);
        var playerDocument = collection.Find(filter).FirstOrDefault();

        if (playerDocument != null)
        {
            // Gets the current score of the player
            double currentScore = playerDocument["score"].AsDouble;

            // Calculates the new score by adding the current score and the score to add
            double newScore = currentScore + scoreToAdd;

            // Updates the score in the database
            var update = Builders<BsonDocument>.Update.Set("score", newScore);
            collection.UpdateOne(filter, update);
        }
        else
        {
            Debug.LogError("Player not found in the database: " + playerName);
        }
    }

    // Retrieves the score of a player from the database
    public string GetScore(string playerName)
    {
        if (collection == null)
        {
            // Initializes the collection if not already initialized
            database = client.GetDatabase("UnityDB");
            collection = database.GetCollection<BsonDocument>("Players");
        }

        // Filters the player's document by name
        var filter = Builders<BsonDocument>.Filter.Eq("player", playerName);
        var playerDocument = collection.Find(filter).FirstOrDefault();

        if (playerDocument != null)
        {
            // Retrieves the player's score
            string score = playerDocument["score"].AsString;
            return score;
        }
        else
        {
            Debug.LogError("Player not found in the database: " + playerName);
            return null; // Indicates player not found
        }
    }

    // Retrieves the top players from the database
    public List<BsonDocument> GetTopPlayers(int limit = 5)
    {
        if (collection == null)
        {
            // Initializes the collection if not already initialized
            database = client.GetDatabase("UnityDB");
            collection = database.GetCollection<BsonDocument>("Players");
        }

        // Sorts players by score in descending order and limits the result to the specified limit
        var sort = Builders<BsonDocument>.Sort.Descending("score");
        var topPlayers = collection.Find(new BsonDocument()).Sort(sort).Limit(limit).ToList();

        return topPlayers;
    }

    // Retrieves a player's score from the database
    public double GetPlayerScore(string playerName)
    {
        if (collection == null)
        {
            // Initializes the collection if not already initialized
            database = client.GetDatabase("UnityDB");
            collection = database.GetCollection<BsonDocument>("Players");
        }
        var filter = Builders<BsonDocument>.Filter.Eq("player", playerName);
        var playerDocument = collection.Find(filter).FirstOrDefault();

        if (playerDocument != null)
        {
            return playerDocument["score"].AsDouble;
        }
        else
        {
            Debug.LogError("Player not found in the database: " + playerName);
            return 0.0; // or throw an exception indicating player not found
        }
    }

}
