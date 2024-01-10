using FluentMigrator;
using Nop.Core.Domain.Catalog;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Nop.Plugin.Widgets.SalesCampaigns.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.SalesCampaigns.Data
{
    [NopMigration("2022/12/15 09:09:17:6455442", "Widgets.SalesCampaigns base schema", MigrationProcessType.Installation)]
    public class SchemaMigration : MigrationBase
    {
        public override void Up()
        {
            Create.TableFor<SalesCampaignTable>();
            Create.TableFor<SalesCampaignsProductMapping>();
        }
        public override void Down()
        {
            //Nothing
        }
    }
}

