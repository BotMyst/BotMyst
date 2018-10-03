﻿// <auto-generated />
using BotMyst.Web.Models.DatabaseContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BotMyst.Web.Migrations.CommandOptions
{
    [DbContext(typeof(CommandOptionsContext))]
    [Migration("20181003144602_LmgtfyDefaultSearchEngine")]
    partial class LmgtfyDefaultSearchEngine
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065");

            modelBuilder.Entity("BotMyst.Shared.Models.CommandOptions.Moderation.ClearOptions", b =>
                {
                    b.Property<ulong>("GuildId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Enabled");

                    b.HasKey("GuildId");

                    b.ToTable("ClearOptions");
                });

            modelBuilder.Entity("BotMyst.Shared.Models.CommandOptions.Utility.AvatarOptions", b =>
                {
                    b.Property<ulong>("GuildId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Enabled");

                    b.HasKey("GuildId");

                    b.ToTable("AvatarOptions");
                });

            modelBuilder.Entity("BotMyst.Shared.Models.CommandOptions.Utility.LmgtfyOptions", b =>
                {
                    b.Property<ulong>("GuildId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DefaultSearchEngine");

                    b.Property<bool>("Enabled");

                    b.HasKey("GuildId");

                    b.ToTable("LmgtfyOptions");
                });
#pragma warning restore 612, 618
        }
    }
}
