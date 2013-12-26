using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tetris;
using Tetris.GameBase;

namespace UnitTestProject
{
    [TestClass]
    public class TetrisItemFactoryTest
    {
        private static TetrisGame _game;
        private static ITetrisFactory _factory;
        [ClassInitialize]
        public static void SetupFactory(TestContext t)
        {
            _game = (new Tetrisor()).NewGame();
            _factory = _game.Factory;
        }
        [TestMethod]
        public void TestGenItemSquare()
        {
            Assert.IsInstanceOfType(_factory,typeof (TetrisItemFactory));
        }
    }
}
