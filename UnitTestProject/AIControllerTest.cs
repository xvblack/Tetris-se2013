using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tetris;
using Tetris.GameBase;

namespace UnitTestProject
{
    /// <summary>
    /// AIControllerTest 的摘要说明
    /// </summary>
    [TestClass]
    public class AIControllerTest
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
        public void TestAILevel()
        {
            AIController _aiController = new AIController(_game, AIController.AIType.High);
            Assert.AreEqual(150, _aiController._speed);
            Assert.AreEqual(0, _aiController._count);
            Assert.AreEqual(0, _aiController._error);
            _aiController = new AIController(_game, AIController.AIType.Middle);
            Assert.AreEqual(12, _aiController._speed);
            Assert.AreEqual(2, _aiController._count);
            Assert.AreEqual(10, _aiController._error);
            _aiController = new AIController(_game, AIController.AIType.Low);
            Assert.AreEqual(12, _aiController._speed);
            Assert.AreEqual(1, _aiController._count);
            Assert.AreEqual(30, _aiController._error);
            _aiController = new AIController(_game);
            Assert.AreEqual(150, _aiController._speed);
            Assert.AreEqual(0, _aiController._count);
            Assert.AreEqual(0, _aiController._error);
        }
        [TestMethod]
        public void TestCalFun()
        {
            AIController _aiController = new AIController(_game);
            List<SquareArray> array = new List<SquareArray>(Square.Styles(Tetrisor.styles));
            Block block = new Block(array[5]) { RPos = _game.Width / 2 - 1 }; // int[2, 3]{{6, 6, 0}, {0, 6, 6}}
            Assert.AreEqual(-34, _aiController.calRating(block)); //  - 0 + 0 - 8 - 22 - 4 * 1 - 0
            Assert.AreEqual(100, _aiController.calPrior(block, _game.Width / 2, 0));
            Assert.AreEqual(101, _aiController.calPrior(block, _game.Width / 2, 1));
            Assert.AreEqual(0, _aiController.calPrior(block, _game.Width / 2 - 1, 0));
        }
    }
}
