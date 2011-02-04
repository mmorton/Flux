using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

namespace WickedNite.Flux
{
    public abstract class ViewModelBase : IViewModel
    {
        public virtual void Notify(string p)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    public static class ViewModelBuilder
    {
        public static Type Create<TViewModel>()
        {
            return Create(typeof(TViewModel));
        }

        public static Type Create(Type viewModelType)
        {
            if (!viewModelType.IsInterface)
                throw new ArgumentException("The supplied view model type must be an interface.");

            var typeName = "ViewModel.Generated." + viewModelType.Name;

            var assemblyName = new AssemblyName { Name = viewModelType.Name + "DynAssembly", Version = new Version(1, 0, 0, 0) };
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name + ".dll");

            var typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.BeforeFieldInit, typeof(ViewModelBase), new Type[] { viewModelType });
            typeBuilder.AddInterfaceImplementation(viewModelType);
            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);            

            foreach (var sourceProperty in viewModelType.GetProperties())
            {
                /* we only support properties that can be get/set */
                if (!sourceProperty.CanRead || !sourceProperty.CanWrite)
                    continue;

                GeneratePropertyAndFieldFor(typeBuilder, sourceProperty);
            }

            var implType = typeBuilder.CreateType();
            return implType;
        }

        private static void GenerateConstructor(TypeBuilder typeBuilder)
        {
            
        }

        private static void GeneratePropertyAndFieldFor(TypeBuilder typeBuilder, PropertyInfo sourceProperty)
        {
            var propertyName = sourceProperty.Name;
            var fieldName = "_" + sourceProperty.Name;
            var getMethodName = "get_" + sourceProperty.Name;
            var setMethodName = "set_" + sourceProperty.Name;

            var fieldBuilder = typeBuilder.DefineField(fieldName, sourceProperty.PropertyType, FieldAttributes.Private);
            var propertyBuilder = typeBuilder.DefineProperty(propertyName, PropertyAttributes.None, sourceProperty.PropertyType, null);
            var getMethodBuilder = typeBuilder.DefineMethod(getMethodName, MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Virtual, sourceProperty.PropertyType, new Type[0]);
            var setMethodBuilder = typeBuilder.DefineMethod(setMethodName, MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Virtual, null, new Type[] { sourceProperty.PropertyType });

            /* get */
            var ilGen = getMethodBuilder.GetILGenerator();            
            ilGen.DeclareLocal(sourceProperty.PropertyType);
            ilGen.Emit(OpCodes.Nop);
            ilGen.Emit(OpCodes.Ldarg_0);
            ilGen.Emit(OpCodes.Ldfld, fieldBuilder);
            ilGen.Emit(OpCodes.Stloc_0);
            ilGen.Emit(OpCodes.Ldloc_0);
            ilGen.Emit(OpCodes.Ret);

            /* set */
            ilGen = setMethodBuilder.GetILGenerator();
            ilGen.Emit(OpCodes.Nop);
            ilGen.Emit(OpCodes.Ldarg_0);
            ilGen.Emit(OpCodes.Ldarg_1);
            ilGen.Emit(OpCodes.Stfld, fieldBuilder);
            ilGen.Emit(OpCodes.Ldarg_0);
            ilGen.Emit(OpCodes.Ldstr, propertyName);
            ilGen.Emit(OpCodes.Callvirt, typeof(ViewModelBase).GetMethod("Notify"));
            ilGen.Emit(OpCodes.Nop);
            ilGen.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getMethodBuilder);
            propertyBuilder.SetSetMethod(setMethodBuilder);
        }
    }
}
