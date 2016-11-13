using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using ApplicationDb.Cor.EntityModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AplicationDb.Cor.Test
{
    [TestClass]
    public class SerializableTest
    {
        [TestMethod]
        public void TestSerializ()
        {
            WorkApproval workApproval=new WorkApproval();
            workApproval.Name = "init user";
            BinaryFormatter binary=new BinaryFormatter();
            using (FileStream fs=new FileStream("D:\\t.data",FileMode.Create))
            {
                binary.Serialize(fs,workApproval);
            }
        }
        [TestMethod]
        public void Deserializ()
        {
            BinaryFormatter binary=new BinaryFormatter();
            using (FileStream fs = new FileStream("D:\\t.data", FileMode.Open))
            {
                WorkApproval wa= binary.Deserialize(fs) as WorkApproval;
              Assert.IsNotNull(wa);
                Console.WriteLine(wa.Name);
            }
        }
    }
}
