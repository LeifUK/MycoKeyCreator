using System;
using PetaPoco.NetCore;

namespace MycoKeyMaker.Library.DBObject
{
    [TableName(Database.TableNames.Species)]
    public class Species : DBObject.IObject
    {
        public Species Clone()
        {
            Species species = new Species();
            species.CopyFrom(this);
            return species;
        }

        public void CopyFrom(Species species)
        {
            id = species.id;
            key_id = species.key_id;
            name = species.name;
            qualified_name = species.qualified_name;
            synonyms = species.synonyms;
            common_name = species.common_name;
            fruiting_body = species.fruiting_body;
            cap = species.cap;
            hymenium = species.hymenium;
            gills = species.gills;
            pores = species.pores;
            spines = species.spines;
            stem = species.stem;
            flesh = species.flesh;
            smell = species.smell;
            taste = species.taste;
            season = species.season;
            distribution = species.distribution;
            habitat = species.habitat;
            spore_print = species.spore_print;
            microscopic_features = species.microscopic_features;
            edibility = species.edibility;
            notes = species.notes;
        }

        public Int64 id { get; set; }
        public Int64 key_id { get; set; }
        public string name { get; set; }
        public string qualified_name { get; set; }
        public string synonyms { get; set; }
        public string common_name { get; set; }
        public string fruiting_body { get; set; }
        public string cap { get; set; }
        public string hymenium { get; set; }
        public string gills { get; set; }
        public string pores { get; set; }
        public string spines { get; set; }
        public string stem { get; set; }
        public string flesh { get; set; }
        public string smell { get; set; }
        public string taste { get; set; }
        public string season { get; set; }
        public string distribution { get; set; }
        public string habitat { get; set; }
        public string spore_print { get; set; }
        public string microscopic_features { get; set; }
        public string edibility { get; set; }
        public string notes { get; set; }
    }
}

