using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public enum Suit
    {
        Club, Diamond, Heart, Spade
    }
    public enum Rank
    {
        Six = 6, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace
    }
    public class Karta
    {
        public readonly Suit suit;
        public readonly Rank rank;
        public Karta(Suit newSuit, Rank newRank)
        {
            suit = newSuit;
            rank = newRank;
        }
        private Karta() { }
        public override string ToString()
        {
            return "\t" + rank + " of " + suit;
        }
        public static bool operator==(Karta c1, Karta c2)
        {
            return (c1.suit == c2.suit) && (c1.rank == c2.rank);
        }
        public static bool operator!=(Karta c1, Karta c2)
        {
            return !(c1 == c2);
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Karta tmp = (Karta)obj;
            if (this == tmp) return true;
            else return false;
        }
        public override int GetHashCode()
        {
            return 9 * (int)rank + (int)suit;
        }
        public static bool operator>(Karta c1, Karta c2)
        {
            return (c1.rank > c2.rank);
        }
        public static bool operator<(Karta c1, Karta c2)
        {
            return !(c1 > c2) && !(c1 == c2);
        }
    }
    public class Deck
    {
        List<Karta> list = new List<Karta>();
        public Deck()
        {
            for (int suitVal = 0; suitVal < 4; suitVal++)
                for (int rankVal = 6; rankVal < 15; rankVal++)
                    list.Add(new Karta((Suit)suitVal, (Rank)rankVal));
        }
        public Karta GetCard(int cardNum)
        {
            if (cardNum >= 0 && cardNum <= 35)
                return list[cardNum];
            else throw new ArgumentOutOfRangeException("Value must be between 0 and 35.");
        }
        public void Shuffle()
        {
            Random rand = new Random();
            for(int i=0; i<list.Count; i++)
            {
                int index = rand.Next(list.Count);
                Karta temp = list[index];
                list[index] = list[i];
                list[i] = temp;
            }
        }
    }
    public class Player
    {
        List<Karta> playerlist;
        private string name;
        public string Name
        {
            get { return name; }
        }
        public List<Karta> PlayerList
        {
            get { return playerlist; }
        }
        private Player() { }
        public Player(string newName)
        {
            name = newName;
            playerlist = new List<Karta>();
        }
        public void ShowCards()
        {
            foreach (Karta card in PlayerList)
                Console.WriteLine(card);
        }
    }
    public class Game
    {
        int curCard;
        Deck myDeck;
        Player[] players;
        public Game()
        {
            curCard = 0;
            myDeck = new Deck();
            myDeck.Shuffle();
        }
        public void SetPlayers(Player[] newPlayers)
        {
            if (newPlayers.Length < 2)
                throw new ArgumentException("Minimum 2 players may play this game.");
            players = newPlayers;
        }
        private void DealHands()
        {
            for (int p = 0; p < players.Length; p++)
                for (int c = 0; c < (int)(36 / players.Length); c++)
                    players[p].PlayerList.Add(myDeck.GetCard(curCard++));
        }
        public int PlayGame()
        {
            if (players == null) return -1;
            DealHands();
            Karta[] mas = new Karta[players.Length];
            for (int p = 0; p < players.Length; p++)
            {
                Console.WriteLine("\t{0}\nCurrent hand:", players[p].Name);
                players[p].ShowCards();
                mas[p] = players[p].PlayerList.First<Karta>();
            }
            int j = 0, k = 0;
            for (int i = 0; i < mas.Length - 1; i++)
                for (int l = i + 1; l < mas.Length; l++)
                    if (mas[i] == mas[l]) k++;
            if (k != 0)
            {
                for (int i = 1; i < players.Length; i++)
                {
                    players[0].PlayerList.AddRange(players[i].PlayerList);
                    players[i].PlayerList.Clear();
                }
                return j;
            }
            else
            {
                Karta max = mas[0];
                for (int i = 1; i < mas.Length; i++)
                    if (mas[i] > max)
                    {
                        max = mas[i];
                        j = i;
                    }
                for (int i = 0; i < j; i++)
                {
                    players[j].PlayerList.AddRange(players[i].PlayerList);
                    players[i].PlayerList.Clear();
                }
                for (int i = j + 1; i < players.Length; i++)
                {
                    players[j].PlayerList.AddRange(players[i].PlayerList);
                    players[i].PlayerList.Clear();
                }
                return j;
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            bool OK = false;
            int choice = -1;
            do
            {
                Console.WriteLine("How many players?");
                string input = Console.ReadLine();
                choice = Convert.ToInt32(input);
                if (choice >= 2) OK = true;
            } while (OK == false);
            Player[] players = new Player[choice];
            for(int p=0; p<players.Length; p++)
            {
                Console.WriteLine("Player {0}, enter name:", p + 1);
                string playerName = Console.ReadLine();
                players[p] = new Player(playerName);
            }
            Game newGame = new Game();
            newGame.SetPlayers(players);
            int whoWon = newGame.PlayGame();
            Console.WriteLine("{0} has won the game!", players[whoWon].Name);
        }
    }
}
