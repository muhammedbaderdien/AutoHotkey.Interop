using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoHotkey.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoHotkey.Interop.Tests
{
    [TestClass]
    public class AutoHotkeyEngineTests
    {
	   private readonly AutoHotkeyEngine _ahk = AutoHotkeyEngine.Instance;

	   [TestMethod]
	   public void Can_set_and_get_variable()
	   {
		  _ahk.SetVar("var1", "awesome");
		  var var1Value = _ahk.GetVar("var1");

		  Assert.AreEqual("awesome", var1Value);
	   }

	   [TestMethod]
	   public void Can_set_variable_with_raw_code()
	   {

		  _ahk.ExecRaw("var2:=\"great\"");
		  var var2Value = _ahk.GetVar("var2");

		  Assert.AreEqual("great", var2Value);
	   }

	   [TestMethod]
	   public void Can_load_and_execute_file_with_function()
	   {
		  _ahk.LoadFile("functions.ahk");
		  var helloFunctionExists = _ahk.FunctionExists("hello_message");
		  Assert.IsTrue(helloFunctionExists);

		  var result = _ahk.ExecFunction("hello_message", "John");
		  Assert.AreEqual("Hello, John", result);
	   }


	   [TestMethod]
	   public void Resetting_engine_kills_variables()
	   {
		  _ahk.SetVar("var3", "12345");
		  _ahk.Reset();

		  var var_after_termination = _ahk.GetVar("var3");
		  Assert.AreEqual(string.Empty, var_after_termination);
	   }

	   [TestMethod]
	   public void Can_change_variable_after_reset()
	   {
		  _ahk.SetVar("var4", "54321");
		  _ahk.Reset();
		  _ahk.SetVar("var4", "55555");

		  var var4Value = _ahk.GetVar("var4");
		  Assert.AreEqual("55555", var4Value);
	   }

	   [TestMethod]
	   public void Can_create_ahk_function_and_return_value_with_raw_code()
	   {
		  _ahk.ExecRaw("calc(n1, n2) {\r\nreturn n1 + n2 \r\n}");
		  var returnValue = _ahk.ExecFunction("calc", "1", "2");

		  Assert.AreEqual("3", returnValue);
	   }

	   [TestMethod]
	   public void Can_return_function_result_with_eval()
	   {
		  //create the function
		  _ahk.ExecRaw("TestFunctionForEval() {\r\n return \"It Works!\" \r\n}");

		  //test the eval
		  var results = _ahk.Eval("TestFunctionForEval()");

		  Assert.AreEqual("It Works!", results);
	   }

	   [TestMethod]
	   public void Can_evaluate_expressions()
	   {
		  Assert.AreEqual("450", _ahk.Eval("100+200*2-50"));
		  Assert.AreEqual("230", _ahk.Eval("20*10+30"));
		  Assert.AreEqual("210", _ahk.Eval("10*20+5*2"));
		  Assert.AreEqual("50", _ahk.Eval("100 - 50"));

		  //Assert.Equal("2", ahk.Eval("10 / 5")); 
		  //TODO: there seems to be a problem with division with ahkdll
	   }

	   [TestMethod]
	   public void Can_auto_exec_in_file()
	   {
		  //loads a file and allows the autoexec section to run
		  //(sets a global variable)
		  _ahk.LoadFile("script_exec.ahk");
		  var scriptVar = _ahk.GetVar("script_exec_var");
		  Assert.AreEqual("jack skellington", scriptVar);

		  //run function inside this file to change global variable
		  //(changes the perviously defined global variable)
		  _ahk.ExecFunction("script_exec_var_change");
		  scriptVar = _ahk.GetVar("script_exec_var");
		  Assert.AreEqual("zero", scriptVar);
	   }

	   [TestMethod]
	   public void Loading_pipes_module_executes_function()
	   {
		  var handlerWasCalled = false;

		  var handler = new Func<string, string>(ahkMessage =>
		  {
			 handlerWasCalled = true;
			 System.Threading.Thread.Sleep(1000);
			 return "OK";
		  });

		  _ahk.InitalizePipesModule(handler);
		  _ahk.ExecRaw("serverResult := SendPipeMessage(\"testing\")");
		  Assert.IsTrue(handlerWasCalled);
	   }
    }
}
