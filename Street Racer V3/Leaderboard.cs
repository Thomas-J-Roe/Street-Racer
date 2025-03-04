using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Street_Racer_V3
{
    class Leaderboard
    {
        private List<(string name, int score)> scores;//creates a list that can store all the scores
        private string filePath = "leaderboard.txt";//stores the file where the leaderboard is
        private string leaderboardFilePath;//stores the file path of the leaderboard

        public Leaderboard(string leaderboardFilePath)//initialises the leaderboard
        {
            this.leaderboardFilePath = leaderboardFilePath;//initalises the file path and the scores
            scores = new List<(string, int)>();//creates a new list of scores
            LoadScores();//stores the scores from the file
        }

        public void AddScore(string name, int score)//method to add a score to the leaderboard
        {
            scores.Add((name, score));//adds the score to the list
            scores = scores.OrderByDescending(s => s.score).Take(10).ToList();//only stores the top 10 scores
            SaveScores();//saves any updated scores
        }
        public void LoadScores()//method to load the scores from the file
        {
            scores.Clear();//empties the scores list
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);//stores the scores, sepearting the score from the name 
                scores = lines.Select(line => 
                    {
                        string[] parts = line.Split(':');
                        if(parts.Length == 2 && int.TryParse(parts[1], out int score))
                        {
                            return (parts[0].Trim(), score);
                        }
                        return ("Unknown", 0);//default response
                    }
                    ).OrderByDescending(s => s.Item2).Take(10).ToList();//only saves top 10 scores by descending scores
            }
        }

        public List<(string name, int score)> TopScores()//method to return the top 10 scores
        {
            return scores;//returns the top 10 scores
        }
        public void SaveScores()//method to save the scores to the file
        {
            File.WriteAllLines(filePath, scores.Select(s => $"{s.name}: {s.score}").ToArray());//writes every element in the list onto a new line in the file
        }
        public void Draw(SpriteBatch spriteBatch, SpriteFont font)//draws the leaderboard
        {
            Vector2 position = new Vector2(10, 0);
            spriteBatch.DrawString(font, "Leaderboard", position, Color.Black);//draws the title
            position +=  new Vector2(100,40);//moves the position to where the scores will start 
            for (int i = 0; i < scores.Count; i++)
            {
                spriteBatch.DrawString(font, $"{i + 1}. {scores[i].name} - {scores[i].score}", position, Color.Black);//writes each score
                position.Y += 30;//moves the position down
            }
            position += new Vector2(-100, 40);//moves the position to where escape message appears
            spriteBatch.DrawString(font, "Press esc to go back to menu", position, Color.Black);
        }
    }
}
