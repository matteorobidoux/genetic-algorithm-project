using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GeneticAlgorithm;

namespace GeneticAlgorithmTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestCtor()
        {
            Assert.ThrowsException<ArgumentException>(new Action(delegate {new Chromosome(0,1,2);}));
            Assert.ThrowsException<ArgumentException>(new Action(delegate {new Chromosome(1,0,2);}));
            Chromosome c1 = new Chromosome(10,5,0);
            Assert.AreEqual(10,c1.Length);
            int[] expectedGenes = new int[]{3,4,3,2,1,2,4,2,4,1};
            for (int i = 0; i < c1.Length; i++)
            {
                Assert.AreEqual(c1[i],expectedGenes[i]);
            }
        }

        [TestMethod]
        public void TestCtorDeepCopy()
        {
            Chromosome c1 = new Chromosome(10,5,0);
            Chromosome c2 = new Chromosome(c1.Genes, 5, 0);
            Assert.AreEqual(10,c2.Length);
            int[] expectedGenes = new int[]{3,4,3,2,1,2,4,2,4,1};
            for (int i = 0; i < c2.Length; i++)
            {
                Assert.AreEqual(c2[i],expectedGenes[i]);
            }
        }

        [TestMethod]
        public void TestIndexerGet()
        {
            Chromosome c1 = new Chromosome(10,5,0);
            Assert.ThrowsException<IndexOutOfRangeException>(new Action(delegate {var num = c1[-1];}));
            Assert.ThrowsException<IndexOutOfRangeException>(new Action(delegate {var num = c1[10];}));
            Assert.AreEqual(3, c1[0]);
            Assert.AreEqual(1, c1[9]);
        }

        [TestMethod]
        public void TestIndexerSet()
        {
            Chromosome c1 = new Chromosome(10,5,0);
            Assert.ThrowsException<IndexOutOfRangeException>(new Action(delegate {c1[-1] = 0;}));
            Assert.ThrowsException<IndexOutOfRangeException>(new Action(delegate {c1[10] = 0;}));
            Assert.ThrowsException<ArgumentOutOfRangeException>(new Action(delegate {c1[0] = 7;}));
            Assert.ThrowsException<ArgumentOutOfRangeException>(new Action(delegate {c1[0] = -2;}));
            c1[0] = 0;
            Assert.AreEqual(0, c1[0]);
            c1[9] = 4;
            Assert.AreEqual(4, c1[9]);
        }

        [TestMethod]
        public void TestCompareTo()
        {
            Chromosome c1 = new Chromosome(10,5,0);
            Chromosome c2 = new Chromosome(10,5,0);
            Chromosome c3 = new Chromosome(10,5,0);
            c1.Fitness = 1;
            c2.Fitness = 1;
            c3.Fitness = 3;
            Assert.AreEqual(1, c1.CompareTo(null));
            Assert.AreEqual(0, c1.CompareTo(c2));
            Assert.AreEqual(1, c1.CompareTo(c3));
            Assert.AreEqual(-1, c3.CompareTo(c1));
        }

        [TestMethod]
        public void TestReproduceIncompatibility()
        {
            Chromosome c1 = new Chromosome(10,5,0);
            Chromosome c2 = new Chromosome(11,5,0);
            Chromosome c3 = new Chromosome(10,6,0);
            Assert.ThrowsException<ArgumentException>(new Action(delegate {c1.Reproduce(c2,0);}));
            Assert.ThrowsException<ArgumentException>(new Action(delegate {c1.Reproduce(c3,0);}));
            Assert.ThrowsException<ArgumentOutOfRangeException>(new Action(delegate {c1.Reproduce(c1,-1);}));
            Assert.ThrowsException<ArgumentOutOfRangeException>(new Action(delegate {c1.Reproduce(c1,2);}));
        }

        [TestMethod]
        public void TestReproduceNoMutation()
        {
            Chromosome c1 = new Chromosome(10,5,0);
            Chromosome c2 = new Chromosome(10,5,1);
            int[] c1Genes = new int[]{3,4,3,2,1,2,4,2,4,1};
            int[] c2Genes = new int[]{1,0,2,3,3,2,1,4,0,3};
            int[] expectedKid1 = new int[]{3,4,3,2,1,2,4,4,0,3};
            int[] expectedKid2 = new int[]{1,0,2,3,3,2,1,2,4,1};
            IChromosome[] kids = c1.Reproduce(c2, 0);
            Assert.AreEqual(2, kids.Length);
            for (int i = 0; i < c1.Length; i++)
            {
                Assert.AreEqual(expectedKid1[i], kids[0][i]);
                Assert.AreEqual(expectedKid2[i], kids[1][i]);
            }
        }

        [TestMethod]
        public void TestReproduceWithMutation()
        {
            Chromosome c1 = new Chromosome(10,5,0);
            Chromosome c2 = new Chromosome(10,5,1);
            int[] c1Genes = new int[]{3,4,3,2,1,2,4,2,4,1};
            int[] c2Genes = new int[]{1,0,2,3,3,2,1,4,0,3};
            int[] expectedKid1 = new int[]{3,4,3,2,1,2,4,4,0,3};
            int[] expectedKid2 = new int[]{1,0,2,3,3,2,1,2,4,1};
            IChromosome[] kids = c1.Reproduce(c2, .5);
            Assert.AreEqual(2, kids.Length);
            for (int i = 0; i < c1.Length; i++)
            {
                Assert.AreEqual(expectedKid1[i], kids[0][i]);
                Assert.AreEqual(expectedKid2[i], kids[1][i]);
            }
        }
    }
}
