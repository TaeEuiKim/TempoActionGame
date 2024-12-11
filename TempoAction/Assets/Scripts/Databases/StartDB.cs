using UnityEngine;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System.Linq;
using System;
using System.Collections.Generic;

public class StartDB : MonoBehaviour
{
    private string IDENTITY_POOL_ID = "ap-northeast-2:d1e3bf28-699c-46af-bbfa-cf8bfa14bc17";
    private RegionEndpoint ENDPOINT = RegionEndpoint.APNortheast2;
    private CognitoAWSCredentials credentials;

    private static DynamoDBContext context;
    private AmazonDynamoDBClient client;

    private static Scores scoreData = new Scores();

    private void Awake()
    {
        UnityInitializer.AttachToGameObject(this.gameObject);
        credentials = new CognitoAWSCredentials(IDENTITY_POOL_ID, ENDPOINT);
        client = new AmazonDynamoDBClient(credentials, ENDPOINT);
        context = new DynamoDBContext(client);

        LoadScore();
    }

    [DynamoDBTable("Score_Table")]
    public class Scores
    {
        [DynamoDBHashKey(AttributeName = "KeyValue")]
        public int number { get; set; }

        [DynamoDBProperty(AttributeName = "Score")]
        public int score { get; set; }
    }

    public static void CreateScore(int _score)
    {
        if (GetScore() >= _score) return;

        Scores s = new Scores
        {
            number = 1,
            score = _score,
        };

        // 점수 DB 저장
        context.SaveAsync(s, (result) =>
        {
            if (result.Exception == null)
            {
                UnityEngine.Debug.LogError("Data Input Success");
            }
            else
            {
                UnityEngine.Debug.LogError(result.Exception);
            }
        });
    }

    public static void LoadScore()
    {
        context.LoadAsync<Scores>(1, (AmazonDynamoDBResult<Scores> result) =>
        {
            if (result.Exception != null)
            {
                UnityEngine.Debug.LogError(result.Exception);
                return;
            }

            scoreData = result.Result;
            if (scoreData != null)
            {
                UnityEngine.Debug.LogError($"Score: {scoreData.score}");
            }
            else
            {
                UnityEngine.Debug.LogError("No data found for the given key.");
            }
        });
    }

    public static int GetScore()
    {
        return scoreData.score;
    }
}
