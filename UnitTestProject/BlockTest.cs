using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tetris;
using Tetris.GameBase;

namespace UnitTestProject
{
    [TestClass]
    public class BlockTest
    {
        private static ITetrisFactory _factory;
        private const int TestNum = 10;
        private static TetrisGame _game;
        [ClassInitialize]
        public static void SetupFactory(TestContext test)
        {
            _game = (new Tetrisor()).NewGame();
            _factory = _game.Factory;
        }
        [TestMethod]
        public void RotateTest()
        {
            for (var i = 0; i < TestNum; i++)
            {
                var b = _factory.GenTetris();
                b.Rotate();
                Assert.AreEqual(1, b.Direction);
                b.Rotate();
                Assert.AreEqual(2, b.Direction);
                b.Rotate();
                Assert.AreEqual(3, b.Direction);
                b.Rotate();
                Assert.AreEqual(0, b.Direction);
                b.CounterRotate();
                Assert.AreEqual(3, b.Direction);
                b.CounterRotate();
                Assert.AreEqual(2, b.Direction);
                b.CounterRotate();
                Assert.AreEqual(1, b.Direction);
                b.CounterRotate();
                Assert.AreEqual(0, b.Direction);
            }
        }

        [TestMethod]
        public void FallTest()
        {
            for (var i = 0; i < TestNum; i++)
            {
                int h = _game.Height-1;
                var b = _factory.GenTetris();
                b.LPos = h;
                while (b.LPos >= 0)
                {
                    Assert.AreEqual(h--,b.LPos);
                    b.Fall();
                }
            }
        }
    }
}
