﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLORM.SQLite;
using NLORM.Core.Attributes;
using System.IO;

namespace NLORM.SQLite.Test
{
    class TestClass
    {
        [ColumnType(DbType.String,"30",false,"0001","this is id comment")]
        public string ID { get; set; }

    }

    class TestClass2
    {
        public string ID { get; set; }

    }

    /// <summary>
    /// Summary description for NLORMSQLiteDbTest
    /// </summary>
    [TestClass]
    public class NLORMSQLiteDbTest
    {
        string connectionString;
        static string filePath;
        public NLORMSQLiteDbTest()
        {
            filePath = "C:\\test.sqlite";
            connectionString = "Data Source="+filePath;
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        //Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void TestInitialize()
        {
            try
            {
                File.Delete(filePath);
            }
            finally
            {

            }
        }

        [TestCleanup()]
        public void TestCleanup()
        {
            try
            {
                File.Delete(filePath);
            }
            finally
            {

            }
        }
        #endregion

        [TestMethod]
        public void TestCreateTable()
        {
            var sqliteDbc = new NLORMSQLiteDb(connectionString);
            sqliteDbc.CreateTable<TestClass>();
        }

        [TestMethod]
        public void TestCreateTableWithoutDef()
        {
            var sqliteDbc = new NLORMSQLiteDb(connectionString);
            sqliteDbc.CreateTable<TestClass2>();
        }

        [TestMethod]
        public void TestDropTable()
        {
            TestCreateTable();
            var sqliteDbc = new NLORMSQLiteDb(connectionString);
            sqliteDbc.DropTable<TestClass>();
        }

        [TestMethod]
        public void TestDropTableWithoutDef()
        {
            TestCreateTableWithoutDef();
            var sqliteDbc = new NLORMSQLiteDb(connectionString);
            sqliteDbc.DropTable<TestClass2>();
        }

        [TestMethod]
        public void TestInsertClass1()
        {

            TestCreateTable();
            var c1 = new TestClass();
            c1.ID = "5555";
            var sqliteDbc = new NLORMSQLiteDb(connectionString);
            sqliteDbc.Insert<TestClass>(c1);
            var result = sqliteDbc.Query<TestClass>("SELECT * FROM  TestClass");
            Assert.AreEqual(result.Count() , 1);
        }

        [TestMethod]
        public void TestInsertClassMutiData()
        {
            TestCreateTable();
            var sqliteDbc = new NLORMSQLiteDb(connectionString);
            for (int i = 0; i < 600; i++)
            {
                var c1 = new TestClass();
                c1.ID = "id"+i.ToString();
                sqliteDbc.Insert<TestClass>(c1);
            }
            var result = sqliteDbc.Query<TestClass>("SELECT * FROM  TestClass");
            Assert.AreEqual(result.Count(), 600);
        }

        [TestMethod]
        public void TestInsertClass2()
        {
            TestCreateTableWithoutDef();
            var c1 = new TestClass();
            c1.ID = "5555";
            var sqliteDbc = new NLORMSQLiteDb(connectionString);
            sqliteDbc.Insert<TestClass2>(c1);
            var result = sqliteDbc.Query<TestClass2>("SELECT * FROM  TestClass2");
            Assert.AreEqual(result.Count(), 1);
        }

        [TestMethod]
        public void TestInsertClass2MutiData()
        {
            TestCreateTableWithoutDef();
            var sqliteDbc = new NLORMSQLiteDb(connectionString);
            for (int i = 0; i < 600; i++)
            {
                var c1 = new TestClass2();
                c1.ID = "id" + i.ToString();
                sqliteDbc.Insert<TestClass2>(c1);
            }
            var result = sqliteDbc.Query<TestClass2>("SELECT * FROM  TestClass2");
            Assert.AreEqual(result.Count(), 600);
        }


    }
}
