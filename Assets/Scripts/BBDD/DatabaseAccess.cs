using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;
using System;

public class DatabaseAccess : MonoBehaviour
{
    MongoClient client = new MongoClient("mongodb+srv://mgalvezdam:miquel123@cluster0.rj5yhrh.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0");
    IMongoDatabase database;
    IMongoCollection<BsonDocument> collection;


    public void SaveName(string name)
    {
        double score = 0;
        database = client.GetDatabase("UnityDB");
        collection = database.GetCollection<BsonDocument>("Players");

        var document = new BsonDocument { { "player", name }, { "score", score } };
        collection.InsertOne(document);
    }

    public void UpdateScore(string playerName, double scoreToAdd)
    {
        if (collection == null)
        {
            // Inicializa la colección si aún no está inicializada
            database = client.GetDatabase("UnityDB");
            collection = database.GetCollection<BsonDocument>("Players");
        }

        // Obtiene el documento del jugador
        var filter = Builders<BsonDocument>.Filter.Eq("player", playerName);
        var playerDocument = collection.Find(filter).FirstOrDefault();

        if (playerDocument != null)
        {
            // Obtiene el puntaje actual del jugador
            double currentScore = playerDocument["score"].AsDouble;

            // Calcula el nuevo puntaje sumando el puntaje actual y el puntaje a agregar
            double newScore = currentScore + scoreToAdd;

            // Actualiza el puntaje en la base de datos
            var update = Builders<BsonDocument>.Update.Set("score", newScore);
            collection.UpdateOne(filter, update);
        }
        else
        {
            Debug.LogError("Jugador no encontrado en la base de datos: " + playerName);
        }
    }

    public string GetScore(string playerName)
    {
        if (collection == null)
        {
            // Inicializa la colección si aún no está inicializada
            database = client.GetDatabase("UnityDB");
            collection = database.GetCollection<BsonDocument>("Players");
        }

        // Filtra el documento del jugador por su nombre
        var filter = Builders<BsonDocument>.Filter.Eq("player", playerName);
        var playerDocument = collection.Find(filter).FirstOrDefault();

        if (playerDocument != null)
        {
            // Obtiene la puntuación del jugador
            string score = playerDocument["score"].AsString;
            return score;
        }
        else
        {
            Debug.LogError("Jugador no encontrado en la base de datos: " + playerName);
            return null; // Indica que el jugador no fue encontrado
        }
    }

    public List<BsonDocument> GetTopPlayers(int limit = 5)
    {
        if (collection == null)
        {
            // Inicializa la colección si aún no está inicializada
            database = client.GetDatabase("UnityDB");
            collection = database.GetCollection<BsonDocument>("Players");
        }

        var sort = Builders<BsonDocument>.Sort.Descending("score");
        var topPlayers = collection.Find(new BsonDocument()).Sort(sort).Limit(limit).ToList();

        return topPlayers;
    }

}

