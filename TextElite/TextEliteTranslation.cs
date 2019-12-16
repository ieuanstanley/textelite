using System;
using System.Collections.Generic;
using System.Text;

namespace TextElite
{
    public class TextEliteTranslation
    {
        //macros
        const uint maxlen = 20;
        const uint tonnes = 0;

        const uint galsize = 256;
        const uint AlienItems = 16;
        const uint lasttrade = AlienItems;

        const uint numforLave = 7;
        const uint numforZaonce = 129;
        const uint numforDiso = 147;
        const uint numforRied = 46;

        plansys[] galaxy = new plansys[galsize];
        uint seed;
        uint rnd_seed;
        bool nativerand;

        uint planetnum;

        public class plansys
        {
            uint x;
            uint y;
            uint economy;
            uint govtype;
            uint techlev;
            uint population;
            uint productivity;
            uint radius;
            uint goatsoupseed;
        }

        public struct tradegood
        {
            public uint baseprice;
            public int gradient;
            public uint basequant;
            public uint maskbyte;
            public uint units;
            public string name;
        }

        public class markettype
        {
            uint[] quantity = new uint[lasttrade + 1];
            uint[] price = new uint[lasttrade + 1];
        }

        //Player Workspace
        uint[] shipshold = new uint[lasttrade + 1];
        uint currentplanet;
        uint galaxynum;
        int cash;
        uint fuel;
        markettype localmarket;
        uint holdspace;

        int fuelcost = 2; //0.2 CR/Light year
        int maxfuel = 70; // 7.0 LY tank

        const uint base0 = 0x5A4A;
        const uint base1 = 0x0248;
        const uint base2 = 0xB753; //Base seed for galaxy 1

        string pairs0 = @"ABOUSEITILETSTONLONUTHNOALLEXEGEZACEBIS
            OUSESARMAINDIREA.ERATENB
            ERALAVETIEDORQUANTEISRION";

        string pairs = @"..LEXEGEZACEBISO
               USESARMAINDIREA.
               ERATENBERALAVETI
               EDORQUANTEISRION"; /* Dots should be nullprint characters */

        string[] govnames = {"Anarchy","Feudal","Multi-gov","Dictatorship",
                    "Communist","Confederacy","Democracy","Corporate State"};

        string[] econnames = {"Rich Ind","Average Ind","Poor Ind","Mainly Ind",
                      "Mainly Agri","Rich Agri","Average Agri","Poor Agri"};

        string[] unitnames = { "t", "kg", "g" };

        /* Data for DB's price/availability generation system */
        /*                   Base  Grad Base Mask Un   Name
                             price ient quant     it              */

        int POLITICALLY_CORRECT = 0;

        tradegood[] commodities =
                   {
                    new tradegood
                        {
                            baseprice = 0x13,
                            gradient = -0x02,
                            basequant = 0x06,
                            maskbyte = 0x01,
                            units = 0,
                            name = "Food        "

                        },
                    new tradegood
                        {
                            baseprice = 0x14,
                            gradient = -0x01,
                            basequant = 0x0A,
                            maskbyte = 0x03,
                            units = 0,
                            name = "Textiles    "

                        },
                    new tradegood
                        {
                            baseprice = 0x41,
                            gradient = -0x03,
                            basequant = 0x02,
                            maskbyte = 0x07,
                            units = 0,
                            name = "Radioactives"

                        },
#if POLITICALLY_CORRECT
                    new tradegood
                        {
                            baseprice = 0x28,
                            gradient = -0x05,
                            basequant = 0xE2,
                            maskbyte = 0x1F,
                            units = 0,
                            name = "Robot Slaves"
                        },
                    new tradegood
                        {
                            baseprice = 0x53,
                            gradient = -0x05,
                            basequant = 0xFB,
                            maskbyte = 0x0F,
                            units = 0,
                            name = "Beverages   "
                        },
#else
                    new tradegood
                        {
                            baseprice = 0x28,
                            gradient = -0x05,
                            basequant = 0xE2,
                            maskbyte = 0x1F,
                            units = 0,
                            name = "Slaves      "
                        },
                    new tradegood
                        {
                            baseprice = 0x53,
                            gradient = -0x05,
                            basequant = 0xFB,
                            maskbyte = 0x0F,
                            units = 0,
                            name = "Liquor/Wines"
                        },
#endif
                    new tradegood
                        {
                            baseprice = 0xC4,
                            gradient = 0x08,
                            basequant = 0x36,
                            maskbyte = 0x03,
                            units = 0,
                            name = "Luxuries    "
                        },
#if POLITICALLY_CORRECT
                    new tradegood
                        {
                            baseprice = 0xEB,
                            gradient = 0x1D,
                            basequant = 0x08,
                            maskbyte = 0x78,
                            units = 0,
                            name = "Rare Species"
                        },
#else
                    new tradegood
                        {
                            baseprice = 0xEB,
                            gradient = 0x1D,
                            basequant = 0x08,
                            maskbyte = 0x78,
                            units = 0,
                            name = "Narcotics   "
                        },
#endif
                    new tradegood
                        {
                            baseprice = 0x9A,
                            gradient = 0x0E,
                            basequant = 0x38,
                            maskbyte = 0x03,
                            units = 0,
                            name = "Computers   "
                        },
                    new tradegood
                        {
                            baseprice = 0x75,
                            gradient = 0x06,
                            basequant = 0x11,
                            maskbyte = 0x1F,
                            units = 0,
                            name = "Machinery   "
                        },
                    new tradegood
                        {
                            baseprice = 0x4E,
                            gradient = 0x01,
                            basequant = 0x11,
                            maskbyte = 0x1F,
                            units = 0,
                            name = "Alloys      "
                        },
                    new tradegood
                        {
                            baseprice = 0x7C,
                            gradient = 0x0D,
                            basequant = 0x1D,
                            maskbyte = 0x07,
                            units = 0,
                            name = "Firearms    "
                        },
                    new tradegood
                        {
                            baseprice = 0xB0,
                            gradient = -0x09,
                            basequant = 0xDC,
                            maskbyte = 0x3F,
                            units = 0,
                            name = "Furs        "
                        },
                    new tradegood
                        {
                            baseprice = 0x20,
                            gradient = -0x01,
                            basequant = 0x35,
                            maskbyte = 0x03,
                            units = 0,
                            name = "Minerals    "
                        },
                    new tradegood
                        {
                            baseprice = 0x61,
                            gradient = -0x01,
                            basequant = 0x42,
                            maskbyte = 0x07,
                            units = 1,
                            name = "Gold        "
                        },
                    new tradegood
                        {
                            baseprice = 0xAB,
                            gradient = -0x02,
                            basequant = 0x37,
                            maskbyte = 0x1f,
                            units = 1,
                            name = "Platinum    "

                        },
                    new tradegood
                        {
                            baseprice = 0x2D,
                            gradient = -0x01,
                            basequant = 0xFA,
                            maskbyte = 0x0F,
                            units = 2,
                            name = "Gem-Stones  "
                        },
                    new tradegood
                        {
                            baseprice = 0x35,
                            gradient = 0x0F,
                            basequant = 0xC0,
                            maskbyte = 0x07,
                            units = 0,
                            name = "Alien Items "
                        },
        };

        string[] tradnames = new string[lasttrade];

        //void goat_soup();

        int nocomms = 14;

        Dictionary<string, Func<string,bool>> commands = new Dictionary<string, Func<string, bool>>
        {
            { "buy",  input => true },
            { "sell", input => true },
            { "fuel", input => true },
            { "jump", input => true },
            { "cash", input => true },
            { "mkt", input => true },
            { "help", input => true },
            { "hold", input => true },
            { "sneak", input => true },
            { "local", input => true },
            { "info", input => true },
            { "galhyp", input => true },
            { "quit", input => true },
            { "rand", input => true }
        };

        static int lastrand = 0;

        void mysrand(int seed)
        {
            var rand = new Random(seed);
            lastrand = rand.Next() -1;
        }

        int myrand()
        {
            int r;
            if (nativerand) r = new Random().Next();
            else
            {
                // As supplied by D McDonnell	from SAS Insititute C
                r = (((((((((((lastrand << 3) - lastrand) << 3)
                + lastrand) << 1) + lastrand) << 4)
                - lastrand) << 1) - lastrand) + 0xe60)
                & 0x7fffffff;
                lastrand = r - 1;
            }

            return r;
        }

        char randbyte() { return (char)(myrand() & 0xFF); }

        uint mymin(uint a, uint b) { if (a < b) return (a); else return (b); }
        void stop(string str)
        {
            Console.WriteLine(str);
            Environment.Exit(0);
        }

        void stripout(string s, char c) /* Remove all c's from string s */
{
            s = s.Remove(c);
}

        public void Run()
        {
            
        }
    }
}
