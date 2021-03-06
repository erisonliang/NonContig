﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NonContig;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace NcTests {

	internal static class NcTestUtils {

		static Random rnd = new Random();

		/// <summary>
		/// Returns an array of the specified length containing random bytes.
		/// </summary>
		/// <param name="count"></param>
		/// <returns></returns>
		public static byte[] RandomBytes(int count) {
			byte[] data = new byte[count];			
			for (int i = 0; i < data.Length; i++) {
				data[i] = (byte)rnd.Next(256);
			}
			return data;
		}
	}

	[TestClass]
	public class NcCollectionTests {

		[TestMethod]
		public void TestAdd_Small() {
			NcByteCollection instance = new NcByteCollection();

			byte[] data = NcTestUtils.RandomBytes(100);
			instance.AddRange(data);

			Assert.IsTrue(instance.SequenceEqual(data));
		}

		[TestMethod]
		public void TestAdd_1() {
			NcByteCollection instance = new NcByteCollection();

			instance.Add(127);

			Assert.IsTrue(instance[0] == 127);
		}

		[TestMethod]
		public void TestAdd_IEnumerable() {
			NcByteCollection instance = new NcByteCollection();

			byte[] data = NcTestUtils.RandomBytes(100);
			instance.AddRange((IEnumerable<byte>)data);

			Assert.IsTrue(instance.SequenceEqual(data));
		}

		[TestMethod]
		public void TestAdd_Medium() {
			NcByteCollection instance = new NcByteCollection();

			byte[] data = NcTestUtils.RandomBytes(5 * 1024);
			instance.AddRange(data);

			Assert.IsTrue(instance.SequenceEqual(data));
		}

		[TestMethod]
		public void TestAdd_Large() {
			NcByteCollection instance = new NcByteCollection();

			byte[] data = NcTestUtils.RandomBytes(17 * 1024);
			instance.AddRange(data);

			Assert.IsTrue(instance.SequenceEqual(data));
		}

		[TestMethod]
		public void TestInsert_Small() {
			NcByteCollection instance = new NcByteCollection();

			byte[] initial = NcTestUtils.RandomBytes(100);
			byte[] inserted = NcTestUtils.RandomBytes(50);
			instance.AddRange(initial);
			instance.InsertRange(10, inserted);

			Assert.IsTrue(instance.SequenceEqual(initial.Take(10).Concat(inserted).Concat(initial.Skip(10))));
		}

		[TestMethod]
		public void TestInsert_Medium() {
			NcByteCollection instance = new NcByteCollection();

			byte[] initial = NcTestUtils.RandomBytes(5 * 1024);
			byte[] inserted = NcTestUtils.RandomBytes(2 * 1024);
			instance.AddRange(initial);
			instance.InsertRange(2048, inserted);

			Assert.IsTrue(instance.SequenceEqual(initial.Take(2048).Concat(inserted).Concat(initial.Skip(2048))));
		}

		[TestMethod]
		public void TestInsert_Large() {
			NcByteCollection instance = new NcByteCollection();

			byte[] initial = NcTestUtils.RandomBytes(5 * 1024);
			byte[] inserted = NcTestUtils.RandomBytes(10 * 1024);
			instance.AddRange(initial);
			instance.InsertRange(5120, inserted);

			Assert.IsTrue(instance.SequenceEqual(initial.Take(5120).Concat(inserted).Concat(initial.Skip(5120))));
		}

		[TestMethod]
		public void TestInsert_IEnumerable() {
			NcByteCollection instance = new NcByteCollection();

			byte[] initial = NcTestUtils.RandomBytes(5 * 1024);
			byte[] inserted = NcTestUtils.RandomBytes(10 * 1024);
			instance.AddRange((IEnumerable<byte>)initial);
			instance.InsertRange(5120, (IEnumerable<byte>)inserted);

			Assert.IsTrue(instance.SequenceEqual(initial.Take(5120).Concat(inserted).Concat(initial.Skip(5120))));
		}

		[TestMethod]
		public void TestInsert_1() {
			NcByteCollection instance = new NcByteCollection();

			byte[] initial = NcTestUtils.RandomBytes(5 * 1024);
			byte[] inserted = new byte[] { 127 };
			instance.AddRange((IEnumerable<byte>)initial);
			instance.Insert(5120, inserted[0]);

			Assert.IsTrue(instance.SequenceEqual(initial.Take(5120).Concat(inserted).Concat(initial.Skip(5120))));
		}

		[TestMethod]
		public void TestInsert_Start() {
			NcByteCollection instance = new NcByteCollection();

			byte[] initial = NcTestUtils.RandomBytes(5 * 1024);
			byte[] inserted = NcTestUtils.RandomBytes(2 * 1024);
			instance.AddRange(initial);
			instance.InsertRange(0, inserted);

			Assert.IsTrue(instance.SequenceEqual(inserted.Concat(initial)));
		}

		[TestMethod]
		public void TestInsert_End() {
			NcByteCollection instance = new NcByteCollection();

			byte[] initial = NcTestUtils.RandomBytes(5 * 1024);
			byte[] inserted = NcTestUtils.RandomBytes(2 * 1024);
			instance.AddRange(initial);
			instance.InsertRange(initial.Length-1, inserted);

			Assert.IsTrue(instance.SequenceEqual(initial.Take(initial.Length - 1).Concat(inserted).Concat(initial.Skip(initial.Length - 1))));
		}

		[TestMethod]
		public void TestRemove_Small() {
			NcByteCollection instance = new NcByteCollection();

			byte[] data = NcTestUtils.RandomBytes(100);
			instance.AddRange(data);
			instance.RemoveRange(10, 50);

			Assert.IsTrue(instance.SequenceEqual(data.Take(10).Concat(data.Skip(10+50))));
		}

		[TestMethod]
		public void TestRemove_Medium() {
			NcByteCollection instance = new NcByteCollection();

			byte[] data = NcTestUtils.RandomBytes(5 * 1024);
			instance.AddRange(data);
			instance.RemoveRange(3*1024, 1536);

			Assert.IsTrue(instance.SequenceEqual(data.Take(3 * 1024).Concat(data.Skip(3 * 1024 + 1536))));
		}

		[TestMethod]
		public void TestRemove_Large() {
			NcByteCollection instance = new NcByteCollection();

			byte[] data = NcTestUtils.RandomBytes(12 * 1024);
			instance.AddRange(data);
			instance.RemoveRange(3 * 1024, 5*1024);

			Assert.IsTrue(instance.SequenceEqual(data.Take(3 * 1024).Concat(data.Skip(3 * 1024 + 5 * 1024))));
		}

		[TestMethod]
		public void TestRemove_1() {
			NcByteCollection instance = new NcByteCollection();

			byte[] data = NcTestUtils.RandomBytes(12 * 1024);
			instance.AddRange(data);
			instance.RemoveAt(3 * 1024);

			Assert.IsTrue(instance.SequenceEqual(data.Take(3 * 1024).Concat(data.Skip(3 * 1024 + 1))));
		}

		[TestMethod]
		public void TestRemove_Start() {
			NcByteCollection instance = new NcByteCollection();

			byte[] data = NcTestUtils.RandomBytes(12 * 1024);
			instance.AddRange(data);
			instance.RemoveRange(0, 5 * 1024);

			Assert.IsTrue(instance.SequenceEqual(data.Skip(5 * 1024)));
		}

		[TestMethod]
		public void TestRemove_End() {
			NcByteCollection instance = new NcByteCollection();

			byte[] data = NcTestUtils.RandomBytes(12 * 1024);
			instance.AddRange(data);
			instance.RemoveRange(7 * 1024, 5 * 1024);

			Assert.IsTrue(instance.SequenceEqual(data.Take(7 * 1024)));
		}

		[TestMethod]
		public void TestClone() {
			NcByteCollection original = new NcByteCollection(NcTestUtils.RandomBytes(12 * 1024));
			NcByteCollection cloned = original.Clone();

			Assert.IsTrue(original.SequenceEqual(cloned));
		}

		[TestMethod]
		public void TestCompact() {
			NcByteCollection data = new NcByteCollection(NcTestUtils.RandomBytes(12 * 1024));

			data.RemoveRange(3 * 1024, 100);
			data.InsertRange(2 * 1024, NcTestUtils.RandomBytes(200));
			data.RemoveRange(5 * 1024, 5 * 1024);
			data.InsertRange(6 * 1024, NcTestUtils.RandomBytes(5 * 1024));
			data.RemoveRange(7 * 1024, 3 * 1024);
#if DEBUG
			long oldBytes = data.BlockLengthTotal;
			Console.WriteLine("Used bytes: {0}, Total bytes: {1}, Blocks: {2}", data.LongCount, data.BlockLengthTotal, data.BlockCount);
#endif
			byte[] snapshot = data.ToArray();
			data.Compact();
#if DEBUG
			Console.WriteLine("Used bytes: {0}, Total bytes: {1}, Blocks: {2}", data.LongCount, data.BlockLengthTotal, data.BlockCount);
#endif
			byte[] compacted = data.ToArray();
			Assert.IsTrue(snapshot.SequenceEqual(compacted));
#if DEBUG
			Assert.AreNotEqual(oldBytes, data.BlockLengthTotal);
#endif
		}

		[TestMethod]
		public void TestBinarySerialization() {
			NcByteCollection original = new NcByteCollection(NcTestUtils.RandomBytes(12 * 1024));
			BinaryFormatter formatter = new BinaryFormatter();
			using (MemoryStream ms = new MemoryStream()) {
				formatter.Serialize(ms, original);
				ms.Position = 0;				
				NcByteCollection deserialized = (NcByteCollection)formatter.Deserialize(ms);
				Assert.IsTrue(original.SequenceEqual(deserialized));
			}
		}

		[TestMethod]
		public void TestXmlSerialization() {
			NcByteCollection original = new NcByteCollection(NcTestUtils.RandomBytes(12 * 1024));
			XmlSerializer xs = new XmlSerializer(typeof(NcByteCollection));
			using (MemoryStream ms = new MemoryStream()) {
				xs.Serialize(ms, original);
				ms.Position = 0;
				NcByteCollection deserialized = (NcByteCollection)xs.Deserialize(ms);
				Assert.IsTrue(original.SequenceEqual(deserialized));				
			}
		}

		[TestMethod]
		public void TestCopyToStream() {
			NcByteCollection data = new NcByteCollection(NcTestUtils.RandomBytes(12 * 1024));
			MemoryStream ms = new MemoryStream();
			data.Copy(3 * 1024, ms, 5 * 1024);

			Assert.IsTrue(ms.ToArray().SequenceEqual(data.Skip(3 * 1024).Take(5 * 1024)));
		}

		[TestMethod]
		public void TestCopyFromStream() {
			NcByteCollection data = new NcByteCollection();
			MemoryStream ms = new MemoryStream(NcTestUtils.RandomBytes(12 * 1024));
			ms.Position = 3 * 1024;
			data.Copy(ms, 0, 5 * 1024);

			Assert.IsTrue(data.SequenceEqual(ms.ToArray().Skip(3 * 1024).Take(5 * 1024)));
		}

		/// <summary>
		/// Tests memory performance by allocating non-contiguous collections until the system runs out of memory. 
		/// This test takes a long time to complete.
		/// </summary>
		[TestMethod]
		public void TestMemoryNc() {
			LinkedList<NcByteCollection> instances = new LinkedList<NcByteCollection>();
			int count = 0;
			while (true) {
				try {
					NcByteCollection data = new NcByteCollection(8 * 4096);
					data.Grow(10 * 1024 * 1024);
					instances.AddLast(data);
					count++;
				}
				catch (OutOfMemoryException) {
					Console.WriteLine("Collections allocated: {0}", count);
					break;
				}
			}
			instances.Clear();
			instances = null;
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}

		/// <summary>
		/// Tests memory performance by allocating contiguous byte arrays until the system runs out of memory.
		/// This test can take a long time to complete.
		/// </summary>
		[TestMethod]
		public void TestMemoryArrays() {
			LinkedList<byte[]> instances = new LinkedList<byte[]>();
			int count = 0;
			while (true) {
				try {
					byte[] data = new byte[10 * 1024 * 1024];
					instances.AddLast(data);
					count++;
				}
				catch (OutOfMemoryException) {
					Console.WriteLine("Arrays allocated: {0}", count);
					break;
				}
			}
			instances.Clear();
			instances = null;
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}

		[TestMethod]
		public void TestSequenceEquals_True() {
			byte[] data = NcTestUtils.RandomBytes(30 * 1000);
			NcByteCollection b1 = new NcByteCollection(data, 4096);
			NcByteCollection b2 = new NcByteCollection(data, 3000);

			Assert.IsTrue(b1.SequenceEqual(b2));
		}

		[TestMethod]
		public void TestSequenceEquals_False() {
			NcByteCollection b1 = new NcByteCollection(NcTestUtils.RandomBytes(30 * 1000));
			NcByteCollection b2 = new NcByteCollection(NcTestUtils.RandomBytes(30 * 1000));

			Assert.IsFalse(b1.SequenceEqual(b2));
		}

		[TestMethod]
		public void TestCopyFromArray_Small() {
			NcByteCollection b1 = new NcByteCollection(NcTestUtils.RandomBytes(30 * 1000));
			byte[] b2 = NcTestUtils.RandomBytes(1000);

			b1.Copy(b2, 0, 5000, 50);
			Assert.IsTrue(b1.Count == (30 * 1000));
			Assert.IsTrue(b1.SequenceEqual(b1.Take(5000).Concat(b2.Take(50)).Concat(b1.Skip(5050))));
		}

		[TestMethod]
		public void TestCopyFromStream_Small() {
			NcByteCollection b1 = new NcByteCollection(NcTestUtils.RandomBytes(30 * 1000));
			NcByteStream b2 = new NcByteStream(NcTestUtils.RandomBytes(1000));

			b1.Copy(b2, 5000, 50);
			Assert.IsTrue(b1.Count == (30 * 1000));
			Assert.IsTrue(b1.SequenceEqual(b1.Take(5000).Concat(b2.Data.Take(50)).Concat(b1.Skip(5050))));
		}

#if DEBUG
		[TestMethod]
		public void TestReserve() {
			NcByteCollection instance = new NcByteCollection();
			long bytes = 11 * 1024;
			instance.Reserve(bytes);

			Assert.AreEqual(instance.BlockLengthTotal, bytes);
		}
#endif
	}

	[TestClass]
	public class NcStreamTests {

		[TestMethod]
		public void TestStreamRead() {
			byte[] data = NcTestUtils.RandomBytes(12 * 1024);
			NcByteStream stream = new NcByteStream(data);
			MemoryStream ms = new MemoryStream();

			stream.Seek(5 * 1024, SeekOrigin.Begin);
			stream.CopyTo(ms);

			Assert.IsTrue(ms.ToArray().SequenceEqual(data.Skip(5 * 1024)));
		}

		[TestMethod]
		public void TestStreamWrite() {
			NcByteCollection original = new NcByteCollection(NcTestUtils.RandomBytes(12 * 1024));
			NcByteStream stream = new NcByteStream(original);
			MemoryStream ms = new MemoryStream(NcTestUtils.RandomBytes(5 * 1024));

			stream.Seek(5 * 1024, SeekOrigin.Begin);
			ms.CopyTo(stream);

			Assert.IsTrue(original.Take(5 * 1024).Concat(ms.ToArray()).Concat(original.Skip(5 * 1024 + 5 * 1024)).SequenceEqual(stream.Data));
		}

		[TestMethod]
		public void TestStreamSetLength() {
			int declLength = 12 * 1024;
			NcByteStream stream = new NcByteStream(NcTestUtils.RandomBytes(declLength));
			Assert.AreEqual(declLength, stream.Length);

			long lessLength = 5 * 1024;
			stream.SetLength(lessLength);
			Assert.AreEqual(lessLength, stream.Length);

			long moreLength = 10 * 1024;
			stream.SetLength(moreLength);
			Assert.AreEqual(moreLength, stream.Length);
		}
	}
}
