using LexiconLang.Language;

namespace LexiconLang.SciFi;

public static class SciFiMeanings
{
    public static readonly MeaningPack Core = new(
        "scifi",
        "1.0.0",
        new Meaning[]
        {
            // Space & Void
            new("void", WordClass.Noun, new[] { "space", "emptiness" }, "void"),
            new("stellar", WordClass.Adjective, new[] { "space", "star" }, "stellar"),
            new("cosmic", WordClass.Adjective, new[] { "space", "vast" }, "cosmic"),
            new("nebula", WordClass.Noun, new[] { "space", "star" }, "nebula"),
            new("pulsar", WordClass.Noun, new[] { "space", "energy" }, "pulsar"),
            new("quasar", WordClass.Noun, new[] { "space", "energy" }, "quasar"),
            new("wormhole", WordClass.Noun, new[] { "space", "travel" }, "wormhole"),
            new("singularity", WordClass.Noun, new[] { "space", "mystery" }, "singularity"),
            new("horizon", WordClass.Noun, new[] { "space", "boundary" }, "horizon"),

            // Technology
            new("drive", WordClass.Noun, new[] { "technology", "propulsion" }, "drive"),
            new("lattice", WordClass.Noun, new[] { "technology", "structure" }, "lattice"),
            new("signal", WordClass.Noun, new[] { "technology", "communication" }, "signal"),
            new("node", WordClass.Noun, new[] { "technology", "network" }, "node"),
            new("relay", WordClass.Noun, new[] { "technology", "communication" }, "relay"),
            new("protocol", WordClass.Noun, new[] { "technology", "communication" }, "protocol"),
            new("algorithm", WordClass.Noun, new[] { "technology", "code" }, "algorithm"),
            new("code", WordClass.Noun, new[] { "technology", "code" }, "code"),
            new("circuit", WordClass.Noun, new[] { "technology", "structure" }, "circuit"),
            new("module", WordClass.Noun, new[] { "technology", "structure" }, "module"),
            new("system", WordClass.Noun, new[] { "technology", "structure" }, "system"),
            new("rig", WordClass.Noun, new[] { "technology", "equipment" }, "rig"),
            new("calibrate", WordClass.Verb, new[] { "technology", "action" }, "calibrate"),
            new("optimize", WordClass.Verb, new[] { "technology", "action" }, "optimize"),
            new("sync", WordClass.Verb, new[] { "technology", "communication" }, "sync"),

            // Energy
            new("plasma", WordClass.Noun, new[] { "energy", "matter" }, "plasma"),
            new("photon", WordClass.Noun, new[] { "energy", "light" }, "photon"),
            new("electron", WordClass.Noun, new[] { "energy", "matter" }, "electron"),
            new("atom", WordClass.Noun, new[] { "energy", "matter" }, "atom"),
            new("molecule", WordClass.Noun, new[] { "energy", "matter" }, "molecule"),
            new("flux", WordClass.Noun, new[] { "energy", "power" }, "flux"),
            new("charge", WordClass.Noun, new[] { "energy", "power" }, "charge"),
            new("radiant", WordClass.Adjective, new[] { "energy", "light" }, "radiant"),
            new("volatile", WordClass.Adjective, new[] { "energy", "danger" }, "volatile"),

            // Biology
            new("hive", WordClass.Noun, new[] { "biology", "collective" }, "hive"),
            new("molt", WordClass.Verb, new[] { "biology", "change" }, "molt"),
            new("android", WordClass.Noun, new[] { "artificial", "entity" }, "android"),
            new("cyborg", WordClass.Noun, new[] { "artificial", "hybrid" }, "cyborg"),
            new("entity", WordClass.Noun, new[] { "biology", "existence" }, "entity"),
            new("organism", WordClass.Noun, new[] { "biology", "life" }, "organism"),
            new("specimen", WordClass.Noun, new[] { "biology", "study" }, "specimen"),
            new("mutant", WordClass.Noun, new[] { "biology", "change" }, "mutant"),
            new("exomorph", WordClass.Noun, new[] { "biology", "alien" }, "exomorph"),
            new("strain", WordClass.Noun, new[] { "biology", "category" }, "strain"),
            new("genesis", WordClass.Noun, new[] { "biology", "creation" }, "genesis"),
            new("evolve", WordClass.Verb, new[] { "biology", "change" }, "evolve"),
            new("spawn", WordClass.Verb, new[] { "biology", "creation" }, "spawn"),
            new("morph", WordClass.Verb, new[] { "biology", "change" }, "morph"),

            // Consciousness
            new("consciousness", WordClass.Noun, new[] { "mind", "awareness" }, "consciousness"),
            new("cognition", WordClass.Noun, new[] { "mind", "thought" }, "cognition"),
            new("intelligence", WordClass.Noun, new[] { "mind", "power" }, "intelligence"),
            new("awareness", WordClass.Noun, new[] { "mind", "perception" }, "awareness"),
            new("neural", WordClass.Adjective, new[] { "mind", "system" }, "neural"),
            new("sentient", WordClass.Adjective, new[] { "mind", "awareness" }, "sentient"),
            new("cognitive", WordClass.Adjective, new[] { "mind", "thought" }, "cognitive"),
            new("aware", WordClass.Adjective, new[] { "mind", "perception" }, "aware"),
            new("synapse", WordClass.Noun, new[] { "mind", "connection" }, "synapse"),
            new("think", WordClass.Verb, new[] { "mind", "action" }, "think"),
            new("perceive", WordClass.Verb, new[] { "mind", "perception" }, "perceive"),

            // Structures
            new("station", WordClass.Noun, new[] { "structure", "habitation" }, "station"),
            new("colony", WordClass.Noun, new[] { "structure", "habitation" }, "colony"),
            new("vessel", WordClass.Noun, new[] { "structure", "travel" }, "vessel"),
            new("shuttle", WordClass.Noun, new[] { "structure", "travel" }, "shuttle"),
            new("reactor", WordClass.Noun, new[] { "structure", "energy" }, "reactor"),
            new("chamber", WordClass.Noun, new[] { "structure", "room" }, "chamber"),
            new("vault", WordClass.Noun, new[] { "structure", "storage" }, "vault"),
            new("sector", WordClass.Noun, new[] { "structure", "area" }, "sector"),
            new("bulkhead", WordClass.Noun, new[] { "structure", "barrier" }, "bulkhead"),
            new("hangar", WordClass.Noun, new[] { "structure", "storage" }, "hangar"),
            new("lab", WordClass.Noun, new[] { "structure", "research" }, "lab"),
            new("compound", WordClass.Noun, new[] { "structure", "area" }, "compound"),
            new("outpost", WordClass.Noun, new[] { "structure", "remote" }, "outpost"),
            new("sanctuary", WordClass.Noun, new[] { "structure", "safety" }, "sanctuary"),

            // Advanced
            new("quantum", WordClass.Adjective, new[] { "advanced", "physics" }, "quantum"),
            new("synthetic", WordClass.Adjective, new[] { "advanced", "artificial" }, "synthetic"),
            new("transcendent", WordClass.Adjective, new[] { "advanced", "beyond" }, "transcendent"),
            new("evolved", WordClass.Adjective, new[] { "advanced", "change" }, "evolved"),
            new("nexus", WordClass.Noun, new[] { "advanced", "connection" }, "nexus"),
            new("paradox", WordClass.Noun, new[] { "advanced", "mystery" }, "paradox"),
            new("anomaly", WordClass.Noun, new[] { "advanced", "mystery" }, "anomaly"),
        });
}
