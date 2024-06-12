﻿using Task2.Languages;

namespace Task2
{
    public class Game
    {
        public Game game { get; set; }

        public int playerNumber;
        public string Word { get; set; }
        public int MinChars { get; set; }
        public int MaxChars { get; set; }
        public ILanguage language { get; set; }
        public MyConsole myConsole { get; set; }
        private string numberRegex = @"^\d+$";
        private string playerNumbers = "^[2-5]$";
        public List<string> Words = new List<string>();
        public List<Player> Players = new List<Player>();
        public List<string> ActivePlayers = new List<string>();
        public List<Player> AllPlayers = new List<Player>();

        public Game()
        {
            this.myConsole = new MyConsole();
            this.language = ChooseLanguage();            
        }

        public async void Start()
        {

            try
            {
                WelcomeText();

                playerNumber = PlayerNumbers();

                EnterWord();

                //load player score form file
                List<Player> AllPlayers = await new File().LoadDataAsync();
                this.language.DataLoaded();

                //add players depends on 'playerNumber' value and set prev score
                for (int i = 1; i <= playerNumber; i++)
                {
                    Players.Add(new Player(this, i));
                    Players[i - 1].SetPrevScore(AllPlayers);
                }

                while (ActivePlayers.Count > 1)
                {
                    for (int i = 0; i < playerNumber; i++)
                    {
                        //delete player form 'ActivePlayers' list if player made a mistake
                        if (ActivePlayers.Any(word => word == Players[i].Name))
                        {
                            if (!Players[i].ReadPlayerWord(this)) language.PlayerIsOut(Players[i].Name);
                        }

                        //finish game if there is only one player in current turn
                        if (ActivePlayers.Count == 1)
                        {
                            Finish();
                            break;
                        }
                    }
                }
            }
            catch
            {
                language.ErrorMessage("Start");
            }

        }
        
        public void WelcomeText()
        {
            language.WelcomeText();
        }

        public async void Finish()
        {
            try
            {
                await new File().SaveDataAsync(Players);
                ShowWinner();
            }
            catch
            {
                language.ErrorMessage("Fihish");
            }
        }

        public void ShowWinner()
        {
            int maxScore = 0;

            try
            {
                List<Player> winnerPlayers = new List<Player>();

                foreach (Player player in Players)
                {
                    if (player.Score > maxScore)
                    {
                        maxScore = player.Score;
                    }
                }
                foreach (Player player in Players)
                {
                    if (player.Score == maxScore)
                    {
                        winnerPlayers.Add(player);
                    }
                }

                if (winnerPlayers.Count > 1)
                {
                    language.ShowWinner(winnerPlayers);
                }
                else
                {
                    language.ShowWinner(winnerPlayers[0]);
                }
            }
            catch
            {
                language.ErrorMessage("ShowWinner");
            }

        }

        public ILanguage ChooseLanguage()
        {
            bool result = false;

            try
            {
                while (!result)
                {
                    myConsole.WriteMessage("Please, choose language:");
                    myConsole.WriteMessage("1. English");
                    myConsole.WriteMessage("2. Русский");

                    string? userChoice = myConsole.ReadMessage(this);

                    if (new DataCheck().CheckWithRegex(userChoice, "^[1-2]$"))
                    {
                        result = true;
                        switch (int.Parse(userChoice))
                        {
                            case 1:
                                language = new English();
                                break;
                            case 2:
                                language = new Russian();
                                break;
                        }
                    }
                }
            }
            catch
            {
                language.ErrorMessage("ChooseLanguage");
            }
            return language;
        }

        public int PlayerNumbers()
        {
            string? strPlayerNumbers;

            do
            {
                language.PlayerNumbers();
                strPlayerNumbers = myConsole.ReadMessage(game);

            } while (new DataCheck().CheckWithRegex(strPlayerNumbers, playerNumbers) != true);

            myConsole.Clear();

            return int.Parse(strPlayerNumbers);
        }

        public void EnterWord()
        {
            string? tmpValue;

            try
            {
                do
                {
                    language.EnterMainWordMinChars();
                    tmpValue = myConsole.ReadMessage(game);

                } while (new DataCheck().CheckWithRegex(tmpValue, this.numberRegex) != true);

                myConsole.Clear();
                this.MinChars = int.Parse(tmpValue);

                bool result = false;
                while (!result)
                {
                    language.EnterMainWordMaxChars(this.MinChars);
                    tmpValue = myConsole.ReadMessage(game);

                    result = new DataCheck().CheckWithRegex(tmpValue, this.numberRegex);

                    if (result) result = (this.MinChars < int.Parse(tmpValue));
                }

                myConsole.Clear();

                this.MaxChars = int.Parse(tmpValue);

                string tmpRegex = language.regex.Replace("+$", "{" + this.MinChars + "," + this.MaxChars + "}$");
                do
                {
                    language.EnterMainWord(this.MinChars, this.MaxChars);
                    this.Word = myConsole.ReadMessage(game);

                } while (new DataCheck().CheckWithRegex(this.Word, tmpRegex) != true);

                myConsole.Clear();
            }
            catch
            {
                language.ErrorMessage("EnterWord");
            }

        }

        public void ShowWords()
        {
            try
            {
                foreach (Player player in Players)
                {
                    language.ShowWords(player);
                }
            }
            catch
            {
                language.ErrorMessage("ShowWords");
            }
        }

        public void ShowScore()
        {
            try
            {
                foreach (Player player in Players)
                {
                    language.ShowScore(player);
                }
            }
            catch
            {
                language.ErrorMessage("ShowScore");
            }
        }

        public void ShowTotalScore()
        {
            try
            {
                foreach (Player player in AllPlayers)
                {
                    Console.WriteLine(player.Name);
                    language.ShowScore(player);
                }
            }
            catch
            {
                language.ErrorMessage("ShowScore");
            }
        }
    }
}

