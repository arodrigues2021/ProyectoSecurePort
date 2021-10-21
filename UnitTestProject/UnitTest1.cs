using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecurePort.Encriptador;
using Rhino.Mocks;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestEncriptador()
        {
            try
            {
                bool result;
                encriptador clave = new encriptador();
                var password = "secureport";
                var passwordEncriptado = clave.Encriptacion(password);
                result = (passwordEncriptado == "CRlkm9T/fnXUcb7qecIOMg==") ? true : false;
                Assert.IsTrue(result == true);
            }
            catch
            {
                Assert.Fail();
            }
           
        }

        [TestMethod]
        public void TestDesencriptador()
        {
            try
            {
                bool result;
                encriptador clave = new encriptador();
                var password = "ni4kxgHo+CnjkkjOQFKsIw==";
                var passwordEncriptado = clave.DesEncriptacion(password); ;
                result = (passwordEncriptado == "secureport") ? true : false;
                Assert.IsTrue(result == true);
            }
            catch
            {
                Assert.Fail();
            }

        }
    }
    
}
