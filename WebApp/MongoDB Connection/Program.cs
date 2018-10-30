using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB_Connection
{
    class Program
    {

        static void Main(string[] args)
        {
            MyDynamicClass myDynamicClass = new MyDynamicClass("SomeClass");
            var myClass = myDynamicClass.CreateObject(new string[3] { "ID", "Name", "Joe" }, new Type[3] { typeof(int), typeof(string), typeof(DateTime)});
            Type type = myClass.GetType();

            foreach (PropertyInfo property in type.GetProperties()) {
                Console.WriteLine(property.Name);
            }
        }

    }

    class MyDynamicClass {
        AssemblyName assemblyName;
        public MyDynamicClass(string ClassName) {
            assemblyName = new AssemblyName(ClassName);
        }
        public object CreateObject(string[] PropertyNames, Type[] Types) {
            if (PropertyNames.Length != Types.Length) {
                Console.WriteLine("All properties should have a type");
            }
            TypeBuilder DynamicClass = CreateClass();
            CreateConstructor(DynamicClass);
            for (int i = 0; i < PropertyNames.Length; i++) {
                CreateProperty(DynamicClass, PropertyNames[i], Types[i]);
            }
            Type type = DynamicClass.CreateType();
            return Activator.CreateInstance(type);
        }

        private TypeBuilder CreateClass() {
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            TypeBuilder typeBuilder = moduleBuilder.DefineType(assemblyName.FullName,
                                                                TypeAttributes.Public |
                                                                TypeAttributes.Class |
                                                                TypeAttributes.AutoClass |
                                                                TypeAttributes.AnsiClass |
                                                                TypeAttributes.BeforeFieldInit |
                                                                TypeAttributes.AutoLayout, null);
            return typeBuilder;
        }

        private void CreateConstructor(TypeBuilder typeBuilder) {
            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);
        }

        private void CreateProperty(TypeBuilder typeBuilder, string propertyName, Type propertyType) {
            FieldBuilder fieldBuilder = typeBuilder.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

            PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);
            MethodBuilder getPropMthdBldr = typeBuilder.DefineMethod("get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
            ILGenerator getIl = getPropMthdBldr.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            MethodBuilder setPropMthdBldr = typeBuilder.DefineMethod("set_" + propertyName,
                  MethodAttributes.Public |
                  MethodAttributes.SpecialName |
                  MethodAttributes.HideBySig,
                  null, new[] { propertyType });

            ILGenerator setIl = setPropMthdBldr.GetILGenerator();
            Label modifyProperty = setIl.DefineLabel();
            Label exitSet = setIl.DefineLabel();

            setIl.MarkLabel(modifyProperty);
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, fieldBuilder);

            setIl.Emit(OpCodes.Nop);
            setIl.MarkLabel(exitSet);
            setIl.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getPropMthdBldr);
            propertyBuilder.SetSetMethod(setPropMthdBldr);
        }
    }
}
