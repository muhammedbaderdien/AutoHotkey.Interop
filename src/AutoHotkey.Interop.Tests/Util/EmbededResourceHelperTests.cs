using AutoHotkey.Interop.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoHotkey.Interop.Tests.Util
{
    [TestClass]
    public class EmbededResourceHelperTests
    {
	   private readonly Assembly _testAssembly = typeof(EmbededResourceHelperTests).Assembly;

	   public EmbededResourceHelperTests()
	   {
		  DeleteExtractFiles();
	   }

	   private static void DeleteExtractFiles()
	   {
		  //delete any physical output files that may have happened
		  if (File.Exists("extract1.txt"))
			 File.Delete("extract1.txt");
		  if (File.Exists("extract2.txt"))
			 File.Delete("extract2.txt");
	   }

	   [TestMethod]
	   public void Can_find_resource_with_only_file_name()
	   {
		  var name = EmbededResourceHelper.FindByName(_testAssembly, "file.txt");
		  Assert.AreEqual("AutoHotkey.Interop.Tests.Util.file.txt", name);
	   }

	   [TestMethod]
	   public void Can_find_resource_with_folder_and_file_name()
	   {
		  var name = EmbededResourceHelper.FindByName(_testAssembly, "Util/file.txt");
		  Assert.AreEqual("AutoHotkey.Interop.Tests.Util.file.txt", name);
	   }

	   [TestMethod]
	   public void Can_find_resource_with_full_name()
	   {
		  var found = EmbededResourceHelper.FindByName(_testAssembly,
			"AutoHotkey.Interop.Tests.Util.file.txt");

		  Assert.AreEqual("AutoHotkey.Interop.Tests.Util.file.txt", found);
	   }

	   [TestMethod]
	   public void Returns_null_for_missing()
	   {
		  var name = EmbededResourceHelper.FindByName(_testAssembly, "missingfile.txt");
		  Assert.IsNull(name);
	   }

	   [TestMethod]
	   public void Returns_null_on_partial_folder_name()
	   {
		  //not Util/file.txt
		  var found = EmbededResourceHelper.FindByName(_testAssembly, "til/file.txt");
		  Assert.IsNull(found);
	   }

	   [TestMethod]
	   public void Can_extract_resouce_to_file()
	   {
		  const string testOutputFile = "extract1.txt";
		  var fullResourceName = EmbededResourceHelper.FindByName(_testAssembly, "file.txt");
		  EmbededResourceHelper.ExtractToFile(_testAssembly, fullResourceName, testOutputFile);

		  var testFileContent = File.ReadAllText("extract1.txt");
		  const string expectedFileContent = "this is a test file.";
		  Assert.AreEqual(expectedFileContent, testFileContent);
	   }

    }
}
