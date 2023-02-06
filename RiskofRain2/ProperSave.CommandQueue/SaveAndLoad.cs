using System;
using System.Collections.Generic;
using System.IO;
using static CommandQueue.QueueManager;

namespace ProperSave.CommandQueue
{
    public static class SaveAndLoad
    {
        public static void Load(string path)
        {
            string[] rawLines = File.ReadAllLines(path);
            for (int i = 0; i < rawLines.Length; i++)
            {
                string[] rawLinesSplit = rawLines[i].Split(',');
                for (int j = 1; j < rawLinesSplit.Length; j += 2)
                {
                    mainQueues[(RoR2.ItemTier)i].Add(new QueueEntry
                    {
                        pickupIndex = new RoR2.PickupIndex(Convert.ToInt32(rawLinesSplit[j + 0])),
                        count = Convert.ToInt32(rawLinesSplit[j + 1])
                    });
                }
            }
        }

        public static void Save(string path)
        {
            string textToWrite = "";
            foreach (KeyValuePair<RoR2.ItemTier, List<QueueEntry>> item in mainQueues)
            {
                string line = "";
                line += (int)item.Key;
                foreach (QueueEntry item1 in item.Value)
                {
                    line += ',';
                    line += item1.pickupIndex.value;
                    line += ',';
                    line += item1.count;
                }
                line += "\n";
                textToWrite += line;
            }
            File.WriteAllText(path, textToWrite);
        }
    }
}
