using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Test
{
    public static class HexUtils
    {
        public static List<HexRecord> Load(string filename)
        {
            var lines = File.ReadAllLines(filename);
            var results = new List<HexRecord>();
            foreach (var line in lines)
            {
                Console.WriteLine(line);
                var hexLine = ParseLine(line);
                var hexRecord = ParseRecord(hexLine);
                results.Add(hexRecord);
            }
            return results;
        }

        private static HexLine ParseLine(string line)
        {
            var match = _regex.Match(line);
            if (!match.Success)
                throw new InvalidOperationException("Can't parse line. Format is invalid.");
            var hexLine = new HexLine
            {
                Start = match.Groups["start"].Value,
                Count = match.Groups["count"].Value,
                Address = match.Groups["address"].Value,
                Type = match.Groups["type"].Value,
                Data = match.Groups["data"].Value,
                Checksum = match.Groups["checksum"].Value
            };
            return hexLine;
        }

        private static HexRecord ParseRecord(HexLine hexLine)
        {
            var hexRecord = new HexRecord();
            hexRecord.Start = hexLine.Start[0];
            hexRecord.Count = Convert.ToInt32(hexLine.Count, 16);
            hexRecord.Address = Convert.ToInt32(hexLine.Address, 16);
            hexRecord.Type = Convert.ToInt32(hexLine.Type, 16);
            hexRecord.Data = Enumerable.Range(0, hexLine.Data.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToInt32(hexLine.Data.Substring(x, 2), 16))
                .ToArray();
            hexRecord.Checksum = Convert.ToInt32(hexLine.Checksum, 16);
            return hexRecord;
        }
        
        private static Regex _regex = new Regex(@"^(?<start>.{1})(?<count>.{2})(?<address>.{4})(?<type>.{2})(?<data>.*)(?<checksum>.{2})$", RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
    }

    public class HexLine
    {
        public string Start { get; set; }
        public string Count { get; set; }
        public string Address { get; set; }
        public string Type { get; set; }
        public string Data { get; set; }
        public string Checksum { get; set; }

        public override string ToString()
        {
            return $"{Start}{Count}{Address}{Type}{Data}{Checksum}";
        }
    }

    public class HexRecord
    {
        public char Start { get; set; }
        public int Count { get; set; }
        public int Address { get; set; }
        public int Type { get; set; }
        public int[] Data { get; set; }
        public int Checksum { get; set; }
        
        public override string ToString()
        {
            return $"Count: {Count}\nAddress: {Address}\nType: {Type}\nData: {string.Join(" ", Data.Select(d => d.ToString("X2")))}\nCheckSum: {Checksum}";
        }
    }
}