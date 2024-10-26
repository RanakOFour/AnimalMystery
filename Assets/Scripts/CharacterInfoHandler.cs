using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CharacterInfoHandler
{
    List<CharacterInfo> m_characterInfo;

    public CharacterInfoHandler(string seed)
    {
        m_characterInfo = new List<CharacterInfo>();

        //Create a file reader
        StreamReader sr = new StreamReader("./Assets/FileData/CharacterInfo.txt");

        //Fill in character relations & appearance patterns
        for (int i = 0; i < 8; i++)
        {
            Debug.Log("Current string partition: " + seed.Substring(i * 8, 8));
            m_characterInfo.Add(new CharacterInfo(sr.ReadLine(), seed.Substring(i * 8, 8), i));
        }

        //Set character relations to be the highest between each pair of generated 'relation levels'
        for (int i = 0; i < 8; i++)
        {
            for (int j = i; j < 8; j++)
            {
                if (m_characterInfo[i].GetRelationWith(j) > m_characterInfo[j].GetRelationWith(i))
                {
                    m_characterInfo[j].SetRelationWith(i, m_characterInfo[i].GetRelationWith(j));
                }
                else
                {
                    m_characterInfo[i].SetRelationWith(j, m_characterInfo[j].GetRelationWith(i));
                }
            }
        }

        //Get a random char from the seed and that digit mod 7 is the murderer
        int murderer = int.Parse(seed[Mathf.RoundToInt(UnityEngine.Random.Range(0, 63))].ToString()) % 7;
        m_characterInfo[murderer].SetMurderer(true);

        //To do!!
        // Alibis and clue generation
        // Deciding the murderer (happened previous evening near (but not at) location X
    }

    public List<int> GetCharactersAtLocation(int roomId, int time)
    {
        List<int> toReturn = new List<int>();
        foreach(CharacterInfo ci in m_characterInfo)
        {
            if(ci.GetCurrentHangout(time) == roomId)
            {
                toReturn.Add(ci.GetId());
            }
        }

        return toReturn;
    }
}
