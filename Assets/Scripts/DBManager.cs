using System.Collections;
using System.Collections.Generic;
using System.Data;
using System;
using System.Data.Common;
using Mono.Data.Sqlite;
using System.Drawing;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class DBManager : MonoBehaviour
{
    public static DBManager Instance { get; private set; }
    private string dbUri = "URI=file:mydb.sqlite";
    private string SQL_COUNT_ELEMNTS = "SELECT count(*) FROM Posiciones;";
    private string SQL_CREATE_POSICIONES = "CREATE TABLE IF NOT EXISTS Posiciones (" +
        "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
        "name TEXT, " +
        "timestamp FLOAT," +
        "vector_x FLOAT, " +
        "vector_y FLOAT, " +
        "vector_z FLOAT);";

    private IDbConnection dbConnection;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        OpenDatabase();
        InitializeDB();
    }

    private void OpenDatabase()
    {
        dbConnection = new SqliteConnection(dbUri);
        dbConnection.Open();
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = "PRAGMA foreign_keys = ON";
        dbCommand.ExecuteNonQuery();
    }

    private void InitializeDB()
    {
        IDbCommand dbCmd = dbConnection.CreateCommand();
        dbCmd.CommandText = SQL_CREATE_POSICIONES;
        dbCmd.ExecuteReader();
        Debug.Log("DB Initialized"); 
    }

    public void SavePosition(CharacterPosition position)
    {
        Debug.Log(position.position.x); 
        string command = $"INSERT INTO Posiciones (name, timestamp, vector_x, vector_y, vector_z) " +
            $"VALUES ('{position.name}', '{position.timestamp}', '{position.position.x}', '{position.position.y}', '{position.position.z}')";
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = command;
        dbCommand.ExecuteNonQuery();
        Debug.Log($"Character Position saved: {position.ToCSV()}"); 
    }

    private void OnDestroy()
    {
        dbConnection.Close();
    }
}
