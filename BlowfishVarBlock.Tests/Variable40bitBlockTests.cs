using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlowfishVarBlock;

namespace BlowfishVarBlock.Tests
{
    [TestClass]
    public class Variable40bitBlockTests
    {
        byte[] key = new byte[] { 0x20, 0x36, 0xac, 0x45, 0xd0 };
        [TestMethod]
        public void UInt64_40Bit_Min()
        {
            const ulong sourceValue = ulong.MinValue;
            ulong encryptedValue = ulong.MinValue;
            ulong targetValue = ulong.MaxValue;

            byte[] payload = new byte[5];
            byte[] buffer = new byte[8];

            Buffer.BlockCopy(BitConverter.GetBytes(sourceValue), 0, payload, 0, 5);

            var encryptor = new Blowfish(key);

            encryptor.Encipher(payload, payload.Length, 40);

            payload.CopyTo(buffer, 0);
            encryptedValue = BitConverter.ToUInt64(buffer, 0);

            // one failure mode would be if the encryption didn't happen at all
            Assert.AreNotEqual(sourceValue, encryptedValue);

            // this is the expected value
            Assert.AreEqual((ulong)1042903492005, encryptedValue);

            // use a separate decryptor instance to ensure no dependency on 
            // encryptor stored state
            var decryptor = new Blowfish(key);
            decryptor.Decipher(payload, payload.Length, 40);

            payload.CopyTo(buffer, 0);
            targetValue = BitConverter.ToUInt64(buffer, 0);

            // the output should convert back tot he original uint
            Assert.AreEqual(sourceValue, targetValue);
        }
        [TestMethod]
        public void UInt64_40Bit_Max()
        {
            const ulong sourceValue = (((ulong)1) << 40) - 1; // 40-bit int MaxValue
            ulong encryptedValue = ulong.MinValue;
            ulong targetValue = ulong.MaxValue;


            byte[] payload = new byte[5];
            byte[] buffer = new byte[8];

            Buffer.BlockCopy(BitConverter.GetBytes(sourceValue), 0, payload, 0, 5);

            var encryptor = new Blowfish(key);

            encryptor.Encipher(payload, payload.Length, 40);

            payload.CopyTo(buffer, 0);
            encryptedValue = BitConverter.ToUInt64(buffer, 0);

            // one failure mode would be if the encryption didn't happen at all
            Assert.AreNotEqual(sourceValue, encryptedValue);

            // this is the expected value
            Assert.AreEqual((ulong)1036168798002, encryptedValue);

            // use a separate decryptor instance to ensure no dependency on 
            // encryptor stored state
            var decryptor = new Blowfish(key);
            decryptor.Decipher(payload, payload.Length, 40);

            payload.CopyTo(buffer, 0);
            targetValue = BitConverter.ToUInt64(buffer, 0);

            // the output should convert back tot he original uint
            Assert.AreEqual(sourceValue, targetValue);
        }
        [TestMethod]
        public void UInt64_40Bit_Mixed()
        {
            const ulong sourceValue = (((ulong)1) << 40) - 10; // 40-bit int MaxValue
            ulong encryptedValue = ulong.MinValue;
            ulong targetValue = ulong.MaxValue;


            byte[] payload = new byte[5];
            byte[] buffer = new byte[8];

            Buffer.BlockCopy(BitConverter.GetBytes(sourceValue), 0, payload, 0, 5);

            var encryptor = new Blowfish(key);

            encryptor.Encipher(payload, payload.Length, 40);

            payload.CopyTo(buffer, 0);
            encryptedValue = BitConverter.ToUInt64(buffer, 0);

            // one failure mode would be if the encryption didn't happen at all
            Assert.AreNotEqual(sourceValue, encryptedValue);

            // this is the expected value
            Assert.AreEqual((ulong)205291341399, encryptedValue);

            // use a separate decryptor instance to ensure no dependency on 
            // encryptor stored state
            var decryptor = new Blowfish(key);
            decryptor.Decipher(payload, payload.Length, 40);

            payload.CopyTo(buffer, 0);
            targetValue = BitConverter.ToUInt64(buffer, 0);

            // the output should convert back tot he original uint
            Assert.AreEqual(sourceValue, targetValue);
        }
        [TestMethod]
        public void Multiple_Blocks()
        {
            byte[] original = new byte[] { 0x20, 0x36, 0x45, 0xac, 0xd0, 0x20, 0x36, 0xac, 0x45, 0xd0, 0x20, 0x36, 0xac, 0x45, 0xd0 };
            byte[] encypheredExpected = new byte[] { 52, 202, 24, 208, 6, 73, 45, 38, 128, 114, 73, 45, 38, 128, 114 };

            byte[] payload = new byte[original.Length];

            Buffer.BlockCopy(original, 0, payload, 0, original.Length);

            var encryptor = new Blowfish(key);

            encryptor.Encipher(payload, payload.Length, 40);

            // one failure mode would be if the encryption didn't happen at all
            CollectionAssert.AreNotEqual(original, payload);

            // this is the expected value
            //CollectionAssert.AreEqual(encypheredExpected, payload);

            // use a separate decryptor instance to ensure no dependency on 
            // encryptor stored state
            var decryptor = new Blowfish(key);
            decryptor.Decipher(payload, payload.Length, 40);

            // the output should convert back to the original array
            CollectionAssert.AreEqual(original, payload);
        }
    }
}
