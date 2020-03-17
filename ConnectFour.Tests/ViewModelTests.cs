using ConnectFour.UI;
using System.Collections.Generic;
using System.Windows.Media;
using Xunit;

namespace ConnectFour.Tests
{
    public class ViewModelTests
    {
        [Fact]
        public async void SetStoneInFullColumn()
        {
            MainWindowViewModel vm = new MainWindowViewModel();
            await vm.RestartGame();

            for (var i = 0; i < 6; i++)
            {
                await vm.SetStone(0);
                Assert.Equal(string.Empty, vm.Message);
            }

            await vm.SetStone(0);
            Assert.Equal("Column is full", vm.Message);
        }

        [Fact]
        public async void PlayerOneWin()
        {
            MainWindowViewModel vm = new MainWindowViewModel();
            await vm.RestartGame();

            for (var i = 0; i < 3; i++)
            {
                await vm.SetStone(0);
                Assert.Equal(string.Empty, vm.Message);
                await vm.SetStone(1);
                Assert.Equal(string.Empty, vm.Message);
            }

            await vm.SetStone(0);
            Assert.Equal("Player 1 wins!", vm.Message);

            await vm.SetStone(0);
            Assert.Equal("Game already over", vm.Message);
        }

        [Fact]
        public async void CheckBoardAfterSet()
        {
            MainWindowViewModel vm = new MainWindowViewModel();
            await vm.RestartGame();

            await vm.SetStone(0);
            Assert.Equal(string.Empty, vm.Message);

            List<List<SolidColorBrush>> board = vm.Board;

            for (int i = 0; i < board.Count; i++)
            {
                for (int j = 0; j < board[i].Count; j++)
                {
                    if (i == 5 && j == 0)
                    {
                        Assert.Equal(Colors.Blue, board[i][j].Color);
                    }
                    else
                    {
                        Assert.Equal(Colors.Gray, board[i][j].Color);
                    }
                }
            }
        }
    }
}
