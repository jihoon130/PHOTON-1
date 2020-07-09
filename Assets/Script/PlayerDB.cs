using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;



public class entity
{
    public string ID { get; set; }
    public string NickName { get; set; }
    public int Score{ get; set; }
}
public class PlayerDB : MonoBehaviour
{
    private MongoClient client;
    private MongoServer server;
    private MongoDatabase db;
    private MongoCollection Players;
    public string ip;
    public List<String> si;
    public List<string> so;
    // Start is called before the first frame update
    void Start()
    {
        ip = GetIP(ADDRESSFAM.IPv6);
        client = new MongoClient(new MongoUrl("mongodb://localhost"));
        server = client.GetServer();
        server.Connect();
        db = server.GetDatabase("joljak");
        Players = db.GetCollection<entity>("joljak");
        isOK();
        AddPlayer();
        UpdateDB(ip, 2000);
    }

    public void InsertDate(string cot, int dis)
    {
        entity Entity = new entity { ID = cot, NickName = PlayerPrefs.GetString("NickName"),Score = dis };
        Players.Insert(Entity);
    }


    public void AddPlayer()
    {
        string l = si.Find(player => player == ip);
        if(l == null)
        {
            InsertDate(ip, 1000);
            isOK();
        }

    }
    public static string GetIP(ADDRESSFAM Addfam)
    {
        if (Addfam == ADDRESSFAM.IPv6 && !Socket.OSSupportsIPv6)
        {
            return null;
        }

        string output = "";

        foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
        {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            NetworkInterfaceType _type1 = NetworkInterfaceType.Wireless80211;
            NetworkInterfaceType _type2 = NetworkInterfaceType.Ethernet;

            if ((item.NetworkInterfaceType == _type1 || item.NetworkInterfaceType == _type2) && item.OperationalStatus == OperationalStatus.Up)
#endif 
            {
                foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                {
                    //IPv4
                    if (Addfam == ADDRESSFAM.IPv4)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            output = ip.Address.ToString();
                        }
                    }

                    //IPv6
                    else if (Addfam == ADDRESSFAM.IPv6)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetworkV6)
                        {
                            output = ip.Address.ToString();
                        }
                    }
                }
            }
        }
        return output;
    }

    public enum ADDRESSFAM
    {
        IPv4, IPv6
    }

    public void isOK()
    {
        var sl = Players.FindAllAs<BsonDocument>();
        foreach (var v in sl)
        {
            BsonValue key;
            if (v.TryGetValue("ID", out key))
            {
                si.Add(key.ToString());
            }
        }
        foreach (var v in sl)
        {
            BsonValue key;
            if (v.TryGetValue("Score", out key))
            {
                so.Add(key.ToString());
            }
        }
    }

    public void UpdateDB(string cot, int dis)
    {
        QueryDocument query = new QueryDocument("ID", ip);
        Players.Remove(query);
        entity Entity = new entity { ID = cot, NickName = PlayerPrefs.GetString("NickName"), Score = dis };
        Players.Insert(Entity);
    }
}
