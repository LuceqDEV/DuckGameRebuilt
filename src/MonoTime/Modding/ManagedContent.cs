﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.ManagedContent
// Assembly: DuckGame, Version=1.1.8175.33388, Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DuckGame
{
    public static class ManagedContent
    {
        public static ManagedContentList<Thing> Things = new ManagedContentList<Thing>();
        public static ManagedContentList<AmmoType> AmmoTypes = new ManagedContentList<AmmoType>();
        public static ManagedContentList<DeathCrateSetting> DeathCrateSettings = new ManagedContentList<DeathCrateSetting>();
        public static ManagedContentList<DestroyType> DestroyTypes = new ManagedContentList<DestroyType>();

        private static void InitializeContentSet<T>(ManagedContentList<T> list)
        {
            if (MonoMain.moddingEnabled)
            {
                foreach (Mod accessibleMod in (IEnumerable<Mod>)ModLoader.accessibleMods)
                {
                    List<System.Type> typeList = accessibleMod.GetTypeList(typeof(T));
                    foreach (System.Type type in accessibleMod.configuration.contentManager.Compile<T>(accessibleMod))
                    {
                        list.Add(type);
                        typeList.Add(type);
                    }
                }
            }
            else
            {
                foreach (System.Type subclass in Editor.GetSubclasses(typeof(T)))
                    list.Add(subclass);
            }
        }

        public static Assembly ResolveModAssembly(object sender, ResolveEventArgs args)
        {
            Mod mod;
            if (ModLoader._modAssemblyNames.TryGetValue(args.Name, out mod))
                return mod.configuration.assembly;
            return args.Name.StartsWith("Steam,") ? ((IEnumerable<Assembly>)AppDomain.CurrentDomain.GetAssemblies()).FirstOrDefault<Assembly>((Func<Assembly, bool>)(x => x.FullName.StartsWith("Steam,") || x.FullName.StartsWith("Steam.Debug,"))) : (Assembly)null;
        }

        public static void PreInitializeMods()
        {
            if (!MonoMain.moddingEnabled)
                return;
            ModLoader.AddMod((Mod)(CoreMod.coreMod = new CoreMod()));
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(ManagedContent.ResolveModAssembly);
            DuckFile.CreatePath(DuckFile.modsDirectory);
            DuckFile.CreatePath(DuckFile.globalModsDirectory);
            ModLoader.PreLoadMods(DuckFile.modsDirectory);
        }

        public static void InitializeMods()
        {
            MonoMain.loadMessage = "Loading Mods";
            if (MonoMain.moddingEnabled)
                ModLoader.LoadMods(DuckFile.modsDirectory);
            MonoMain.currentActionQueue.Enqueue(new LoadingAction((Action)(() =>
           {
               ModLoader.InitializeAssemblyArray();
               ManagedContent.InitializeContentSet<Thing>(ManagedContent.Things);
               ManagedContent.InitializeContentSet<AmmoType>(ManagedContent.AmmoTypes);
               ManagedContent.InitializeContentSet<DeathCrateSetting>(ManagedContent.DeathCrateSettings);
               ManagedContent.InitializeContentSet<DestroyType>(ManagedContent.DestroyTypes);
               ContentProperties.InitializeBags(ManagedContent.Things.Types);
           })));
        }
    }
}