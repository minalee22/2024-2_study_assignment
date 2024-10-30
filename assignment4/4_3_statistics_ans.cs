using System;
using System.Linq;

namespace statistics
{
    class Program
    {
        static void Main(string[] args)
        {
            string[,] data = {
                {"StdNum", "Name", "Math", "Science", "English"},
                {"1001", "Alice", "85", "90", "78"},
                {"1002", "Bob", "92", "88", "84"},
                {"1003", "Charlie", "79", "85", "88"},
                {"1004", "David", "94", "76", "92"},
                {"1005", "Eve", "72", "95", "89"}
            };
            // You can convert string to double by
            // double.Parse(str)

            int stdCount = data.GetLength(0) - 1;
            int sjtCount = data.GetLength(1) - 2;

            string[] names = new string[stdCount];
            double[] total = new double[stdCount];
            double[] avg = new double[sjtCount];
            double[] max = new double[sjtCount];
            double[] min = new double[sjtCount];

            for (int j = 0; j < sjtCount; j++){
                max[j] = double.MinValue;
                min[j] = double.MaxValue;
            }

            for (int i = 1; i <= stdCount; i++)
            {
                double totalScore = 0;
                names[i - 1] = data[i, 1];
                for (int j = 2; j < sjtCount + 2; j++)
                {
                    double score = double.Parse(data[i, j]);
                    totalScore += score;
                    avg[j - 2] += score / stdCount;
                    if (score > max[j - 2]) 
                        max[j - 2] = score;
                    if (score < min[j - 2]) 
                        min[j - 2] = score;
                }
                total[i - 1] = totalScore;
            }

            Console.WriteLine("Average Scores:");
            for(int j = 2; j < sjtCount + 2; j++){
                Console.WriteLine($"{data[0, j]}: {avg[j - 2]:F2}");
            }
            Console.WriteLine("\nMax and min Scores:");
            for(int j = 2; j < data.GetLength(1); j++){
                Console.WriteLine($"{data[0, j]}: ({max[j - 2]}, {min[j - 2]})");
            }

            Array.Sort(total, names);
            Array.Reverse(total);
            Array.Reverse(names);

            Console.WriteLine("\nStudents rank by total scores:");
            for(int i = 0; i < stdCount; i++){
                int rank = i + 1;
                if(rank == 1)
                    Console.WriteLine($"{names[i]}: {rank}st");
                else if(rank == 2)
                    Console.WriteLine($"{names[i]}: {rank}nd");
                else if(rank == 3)
                    Console.WriteLine($"{names[i]}: {rank}rd");
                else
                    Console.WriteLine($"{names[i]}: {rank}th");
            }
        }
    }
}

/* example output

Average Scores: 
Math: 84.40
Science: 86.80
English: 86.20

Max and min Scores: 
Math: (94, 72)
Science: (95, 76)
English: (92, 78)

Students rank by total scores:
Alice: 4th
Bob: 1st
Charlie: 5th
David: 2nd
Eve: 3rd

*/
