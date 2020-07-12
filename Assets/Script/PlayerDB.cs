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
using UnityEngine.UI;


public class entity
{
    public string ID { get; set; }
    public string NickName { get; set; }
    public int Score { get; set; }
}
public class PlayerDB : MonoBehaviour
{
    private MongoClient client;
    private MongoServer server;
    private MongoDatabase db;
    private MongoCollection Players;
    public string ip;
    public Text[] TopPlayer;
    public Text RakingText;
    public Dictionary<string, int> oo = new Dictionary<string, int>();
    public int UserScore;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        ip = GetIP(ADDRESSFAM.IPv6);
        client = new MongoClient(new MongoUrl("mongodb://14.35.214.221"));
        server = client.GetServer();
        if(server==null)
        {
            Debug.Log("Close");
            return;
        }

        server.Connect();
        db = server.GetDatabase("joljak");
        Players = db.GetCollection<entity>("joljak");
        isOK();
        AddPlayer();
        InvokeRepeating("GetScore", 1.0f, 0.1f);
    }

    public void InsertDate(string cot, int dis)
    {
        entity Entity = new entity { ID = cot, NickName = PlayerPrefs.GetString("NickName"), Score = dis };
        Players.Insert(Entity);
    }


    public void AddPlayer()
    {
        int Score2;
        if (!oo.TryGetValue(ip, out Score2))
        {
            InsertDate(ip, 1000);
            isOK();
        }
    }

    public void GetScore()
    {
        int Score2;
        if (oo.TryGetValue(ip, out Score2))
        {
            UserScore = Score2;
            CancelInvoke("GetScore");
        }
    }

    public void Ranking()
    {
        if (server == null)
            return;

        int count = 1;
        RakingText.text = "";
        oo = oo.OrderByDescending(node => node.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
        foreach (var d in oo)
        {

            var sl = Players.FindAllAs<BsonDocument>();
            foreach (var v in sl)
            {
                BsonValue key;
                if (v.TryGetValue("ID", out key))
                {
                    BsonValue key1;
                    if (v.TryGetValue("NickName", out key1))
                    {
                        if (key.ToString() == d.Key)
                        {
                            switch (count)
                            {
                                case 1:
                                    TopPlayer[0].text = key1.ToString() + "\n" + d.Value;
                                    count++;
                                    break;
                                case 2:
                                    TopPlayer[1].text = key1.ToString() + "\n" + d.Value;
                                    count++;
                                    break;
                                case 3:
                                    TopPlayer[2].text = key1.ToString() + "\n" + d.Value;
                                    count++;
                                    break;
                                default:
                                    RakingText.text += count + "등" + "\t\t\t" + key1.ToString() + "\t\t\t" + d.Value + "\n";
                                    count++;
                                    break;

                            }
                        }
                    }
                }
            }
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
        foreach (var key in oo.Keys.ToList())
        {
            oo.Remove(key);
        }

        var sl = Players.FindAllAs<BsonDocument>();
        foreach (var v in sl)
        {
            BsonValue key;
            if (v.TryGetValue("ID", out key))
            {
                BsonValue key1;
                if (v.TryGetValue("Score", out key1))
                {
                    oo.Add(key.ToString(), int.Parse(key1.ToString()));
                }
            }
        }
    }

    public void UpdateDB(string cot, int dis)
    {
        if (server == null)
            return;

        QueryDocument query = new QueryDocument("ID", ip);
        Players.Remove(query);
        entity Entity = new entity { ID = cot, NickName = PlayerPrefs.GetString("NickName"), Score = dis };
        Players.Insert(Entity);
    }
}
