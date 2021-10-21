using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using SecurePort;
using SecurePort.Services.Interfaces;
using SecurePort.Entities.Models;

namespace UnitTestProject
{

    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        //Consultamos los Usuarios activos
        public void TestGetAllUsuarios()
        {
            try
            {
                var service = new ServiciosUsuarios(MockRepository.GenerateMock<ILogger>());
                var listUsers = service.GetAllUsuarios();
                Assert.IsNotNull(listUsers);
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        //Consultamos todos los usuarios del sistema
        public void TestGetAll()
        {
            try
            {
                var service = new ServiciosUsuarios(MockRepository.GenerateMock<ILogger>());
                var listUsers = service.GetAllUsuarios();
                Assert.IsNotNull(listUsers);
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        //Consultamos si existe el email
        public void TestGetEmailOK()
        {
            try
            {
                var service = new ServiciosUsuarios(MockRepository.GenerateMock<ILogger>());
                var email = "arodrigues2010@hotmail.com";
                bool valor = service.GetEmail(email);
                Assert.IsTrue(valor);
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        //Consultamos si existe el email
        public void TestGetEmailKO()
        {
            try
            {
                var service = new ServiciosUsuarios(MockRepository.GenerateMock<ILogger>());
                var email = "xxxxxxxxxx@hotmail.com";
                bool valor = service.GetEmail(email);
                Assert.IsFalse(valor);
            }
            catch
            {
                Assert.Fail();
            }
        }
    }
}