﻿using System.Data.Common;
using System.Data.Common.CommandTrees;
using System.Data.Metadata.Edm;

namespace Glimpse.EF.Plumbing.Profiler
{
    internal class GlimpseProfileDbProviderServices : DbProviderServices
    { 
        public GlimpseProfileDbProviderServices(DbProviderServices innerProviderServices, ProviderStats stats)
        {
            InnerProviderServices = innerProviderServices;
            Stats = stats;
        }


        private DbProviderServices InnerProviderServices { get; set; }
        private ProviderStats Stats { get; set; }


        public override DbCommandDefinition CreateCommandDefinition(DbCommand prototype)
        {
            return new GlimpseProfileDbCommandDefinition(InnerProviderServices.CreateCommandDefinition(prototype), Stats);
        }

        protected override DbCommandDefinition CreateDbCommandDefinition(DbProviderManifest providerManifest, DbCommandTree commandTree)
        {
            return new GlimpseProfileDbCommandDefinition(InnerProviderServices.CreateCommandDefinition(commandTree), Stats);
        }

        protected override void DbCreateDatabase(DbConnection connection, int? commandTimeout, StoreItemCollection storeItemCollection)
        {
            InnerProviderServices.CreateDatabase(((GlimpseProfileDbConnection)connection).InnerConnection, commandTimeout, storeItemCollection);
        }

        protected override string DbCreateDatabaseScript(string providerManifestToken, StoreItemCollection storeItemCollection)
        {
            return InnerProviderServices.CreateDatabaseScript(providerManifestToken, storeItemCollection);
        }

        protected override bool DbDatabaseExists(DbConnection connection, int? commandTimeout, StoreItemCollection storeItemCollection)
        {
            return InnerProviderServices.DatabaseExists(((GlimpseProfileDbConnection)connection).InnerConnection, commandTimeout, storeItemCollection);
        }

        protected override void DbDeleteDatabase(DbConnection connection, int? commandTimeout, StoreItemCollection storeItemCollection)
        {
            InnerProviderServices.DeleteDatabase(((GlimpseProfileDbConnection)connection).InnerConnection, commandTimeout, storeItemCollection);
        }

        protected override DbProviderManifest GetDbProviderManifest(string manifestToken)
        {
            return InnerProviderServices.GetProviderManifest(manifestToken);
        }

        protected override string GetDbProviderManifestToken(DbConnection connection)
        {
            return InnerProviderServices.GetProviderManifestToken(((GlimpseProfileDbConnection)connection).InnerConnection);
        } 
    }
}