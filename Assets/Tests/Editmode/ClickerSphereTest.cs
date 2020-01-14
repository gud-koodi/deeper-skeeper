using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using DarkRift;

namespace Tests {
    public class ClickerSphereTest {

        [Test]
        public void ClickerSphereNetworkReadWriteSameLength() {
            // Get the component as properly working Unity component
            GameObject go = new GameObject();
            go.AddComponent<ClickerSphere>();
            ClickerSphere component = go.GetComponent<ClickerSphere>();

            // Check how much data is
            int writtenLength = 0, readLength = 0;
            using (DarkRiftWriter writer = DarkRiftWriter.Create()) {
                writtenLength = writer.Position;
                component.Write(writer);
                writtenLength = writer.Position;

                // Add some padding if result is uneven. Not sure if necessary.
                byte[] padding = new byte[12];
                writer.WriteRaw(padding, writer.Position, padding.Length);
                byte[] writerArray = writer.ToArray();
                
                using (DarkRiftReader reader = DarkRiftReader.CreateFromArray(writerArray, 0, writerArray.Length)) {
                    component.Read(reader);
                    readLength = reader.Position;
                }
            }

            Assert.True(writtenLength == readLength);
        }
    }
}
