using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ConnectFour.UI
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private List<List<SolidColorBrush>> BoardValue = new List<List<SolidColorBrush>>();
        public List<List<SolidColorBrush>> Board
        {
            get => BoardValue;
            set
            {
                BoardValue = value;
                OnPropertyChanged(nameof(Board));
            }
        }

        private string MessageValue = "";
        public string Message
        {
            get => MessageValue;
            set
            {
                MessageValue = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly HttpClient HttpClient = new HttpClient
        {
            BaseAddress = new Uri("https://tbrychconnectfour.azurewebsites.net"),
        };

        public async void Init()
        {
            await RestartGame();
            GetBoard();
        }

        private async void GetBoard()
        {
            var boardString = await HttpClient.GetStringAsync("/connectfour/board");
            System.Diagnostics.Debug.WriteLine(boardString);
            byte[,] res = JsonConvert.DeserializeObject<byte[,]>(boardString);

            List<List<SolidColorBrush>> cBoard = new List<List<SolidColorBrush>>();
            for (int i = 0; i < 6; i++)
            {
                cBoard.Add(new List<SolidColorBrush>());
                for (int j = 0; j < 7; j++)
                {
                    byte curVal = res[i, j];
                    if (curVal == 0)
                    {
                        cBoard[i].Add(new SolidColorBrush(Colors.Gray));
                    }
                    else if (curVal == 1)
                    {
                        cBoard[i].Add(new SolidColorBrush(Colors.Blue));
                    }
                    else if (curVal == 2)
                    {
                        cBoard[i].Add(new SolidColorBrush(Colors.Red));
                    }
                    System.Diagnostics.Debug.Write(res[i, j] + " ");
                }
                System.Diagnostics.Debug.WriteLine("");
            }

            Board = cBoard;
        }

        private async Task RestartGame()
        {
            await HttpClient.PutAsync("/connectfour/restart", null);
        }

        public async void SetStone(int column)
        {
            var rowResponse = await HttpClient.PostAsync($"/connectfour/set/{column}", null);
            var res = await rowResponse.Content.ReadAsStringAsync();
            GetBoard();

            if (res.Equals("1") || res.Equals("2"))
            {
                Message = $"Player {res} wins!";
                return;
            }
            if (!rowResponse.IsSuccessStatusCode)
            {
                Message = res;
                return;
            }
            Message = "";
        }
    }
}
