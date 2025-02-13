using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PasswordScript : MonoBehaviour
{
    private static System.Random random = new System.Random();
    private string password;


    public void Start()
    {
        password = RandomString(6);
        Debug.Log(password);
    }

    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public string getPassword() { return password; }

    public string partialPassword()
    {
        var rd = new System.Random();
        var partial = new StringBuilder();
        foreach(var c in password)
        {
            partial.Append(rd.Next(1, 101) > 75 ? c : "_" ); 
            partial.Append(' ');
        }

        return partial.ToString();
    }

}
