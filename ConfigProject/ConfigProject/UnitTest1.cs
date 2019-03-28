using NUnit.Framework;
using System;
using System.Configuration;

namespace ConfigProject
{

    public class UnitTest1
    {
        [Test]
        public void ValidarAmbientes()
        {

            Console.WriteLine("Nome Branch: " + ConfigurationManager.AppSettings["nome_branch"].ToString());
            Console.WriteLine("Database: " + ConfigurationManager.AppSettings["database"].ToString());

        }
    }
}
