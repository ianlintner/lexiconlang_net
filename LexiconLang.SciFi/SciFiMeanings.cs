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
            new("rift", WordClass.Noun, new[] { "space", "danger" }, "rift"),
            new("ascend", WordClass.Verb, new[] { "advanced", "power" }, "ascend"),
            new("transcend", WordClass.Verb, new[] { "advanced", "power" }, "transcend"),

            // Relationships & Collective
            new("swarm", WordClass.Noun, new[] { "collective", "group" }, "swarm"),
            new("collective", WordClass.Noun, new[] { "collective", "group" }, "collective"),
            new("network", WordClass.Noun, new[] { "collective", "connection" }, "network"),
            new("symbiotic", WordClass.Adjective, new[] { "collective", "bond" }, "symbiotic"),
            new("unified", WordClass.Adjective, new[] { "collective", "bond" }, "unified"),
            new("synergic", WordClass.Adjective, new[] { "collective", "action" }, "synergic"),
            new("link", WordClass.Verb, new[] { "connection", "action" }, "link"),
            new("merge", WordClass.Verb, new[] { "collective", "action" }, "merge"),
            new("integrate", WordClass.Verb, new[] { "connection", "action" }, "integrate"),

            // Danger & Corruption
            new("virus", WordClass.Noun, new[] { "danger", "corruption" }, "virus"),
            new("mutation", WordClass.Noun, new[] { "biology", "danger" }, "mutation"),
            new("parasitic", WordClass.Adjective, new[] { "danger", "biology" }, "parasitic"),
            new("corrupted", WordClass.Adjective, new[] { "danger", "corruption" }, "corrupted"),
            new("breach", WordClass.Noun, new[] { "danger", "failure" }, "breach"),
            new("contamination", WordClass.Noun, new[] { "danger", "corruption" }, "contamination"),
            new("malfunction", WordClass.Noun, new[] { "danger", "failure" }, "malfunction"),
            new("plague", WordClass.Noun, new[] { "danger", "biology" }, "plague"),
            new("infect", WordClass.Verb, new[] { "danger", "action" }, "infect"),
            new("corrupt", WordClass.Verb, new[] { "danger", "action" }, "corrupt"),
            new("decay", WordClass.Verb, new[] { "danger", "time" }, "decay"),

            // Authority & Hierarchy
            new("commander", WordClass.Noun, new[] { "authority", "power" }, "commander"),
            new("council", WordClass.Noun, new[] { "authority", "group" }, "council"),
            new("directive", WordClass.Noun, new[] { "authority", "order" }, "directive"),
            new("mandate", WordClass.Noun, new[] { "authority", "order" }, "mandate"),
            new("sovereign", WordClass.Noun, new[] { "authority", "power" }, "sovereign"),
            new("hierarchy", WordClass.Noun, new[] { "authority", "structure" }, "hierarchy"),
            new("rank", WordClass.Noun, new[] { "authority", "order" }, "rank"),
            new("decree", WordClass.Noun, new[] { "authority", "order" }, "decree"),

            // War & Conflict
            new("strike", WordClass.Verb, new[] { "conflict", "action" }, "strike"),
            new("assault", WordClass.Noun, new[] { "conflict", "action" }, "assault"),
            new("defense", WordClass.Noun, new[] { "conflict", "protection" }, "defense"),
            new("siege", WordClass.Noun, new[] { "conflict", "action" }, "siege"),
            new("tactical", WordClass.Adjective, new[] { "conflict", "strategy" }, "tactical"),
            new("hostile", WordClass.Adjective, new[] { "conflict", "danger" }, "hostile"),
            new("fortify", WordClass.Verb, new[] { "conflict", "protection" }, "fortify"),
            new("deploy", WordClass.Verb, new[] { "conflict", "action" }, "deploy"),

            // Exploration & Discovery
            new("frontier", WordClass.Noun, new[] { "exploration", "boundary" }, "frontier"),
            new("expedition", WordClass.Noun, new[] { "exploration", "action" }, "expedition"),
            new("probe", WordClass.Noun, new[] { "exploration", "technology" }, "probe"),
            new("scan", WordClass.Verb, new[] { "exploration", "action" }, "scan"),
            new("map", WordClass.Verb, new[] { "exploration", "action" }, "map"),
            new("navigate", WordClass.Verb, new[] { "exploration", "action" }, "navigate"),
            new("discover", WordClass.Verb, new[] { "exploration", "action" }, "discover"),

            // Resource & Economy
            new("ore", WordClass.Noun, new[] { "resource", "material" }, "ore"),
            new("mineral", WordClass.Noun, new[] { "resource", "material" }, "mineral"),
            new("fuel", WordClass.Noun, new[] { "resource", "power" }, "fuel"),
            new("commodity", WordClass.Noun, new[] { "resource", "trade" }, "commodity"),
            new("trade", WordClass.Verb, new[] { "economy", "action" }, "trade"),
            new("mine", WordClass.Verb, new[] { "resource", "action" }, "mine"),
            new("harvest", WordClass.Verb, new[] { "resource", "action" }, "harvest"),
            new("refine", WordClass.Verb, new[] { "technology", "action" }, "refine"),

            // Time & Duration
            new("cycle", WordClass.Noun, new[] { "time", "repetition" }, "cycle"),
            new("epoch", WordClass.Noun, new[] { "time", "period" }, "epoch"),
            new("era", WordClass.Noun, new[] { "time", "period" }, "era"),
            new("moment", WordClass.Noun, new[] { "time", "instant" }, "moment"),
            new("phase", WordClass.Noun, new[] { "time", "change" }, "phase"),
            new("temporal", WordClass.Adjective, new[] { "time", "existence" }, "temporal"),
            new("eternal", WordClass.Adjective, new[] { "time", "infinite" }, "eternal"),
            new("transient", WordClass.Adjective, new[] { "time", "temporary" }, "transient"),

            // Weapons & Defense
            new("cannon", WordClass.Noun, new[] { "weapon", "technology" }, "cannon"),
            new("laser", WordClass.Noun, new[] { "weapon", "energy" }, "laser"),
            new("missile", WordClass.Noun, new[] { "weapon", "projectile" }, "missile"),
            new("shield", WordClass.Noun, new[] { "defense", "protection" }, "shield"),
            new("armor", WordClass.Noun, new[] { "defense", "protection" }, "armor"),
            new("turret", WordClass.Noun, new[] { "weapon", "structure" }, "turret"),
            new("drone", WordClass.Noun, new[] { "weapon", "artificial" }, "drone"),

            // Status & Condition
            new("active", WordClass.Adjective, new[] { "status", "state" }, "active"),
            new("dormant", WordClass.Adjective, new[] { "status", "state" }, "dormant"),
            new("critical", WordClass.Adjective, new[] { "status", "danger" }, "critical"),
            new("stable", WordClass.Adjective, new[] { "status", "safe" }, "stable"),
            new("prime", WordClass.Adjective, new[] { "status", "excellent" }, "prime"),
            new("damaged", WordClass.Adjective, new[] { "status", "broken" }, "damaged"),
            new("intact", WordClass.Adjective, new[] { "status", "safe" }, "intact"),
            new("functional", WordClass.Adjective, new[] { "status", "working" }, "functional"),

            // Quality & Description
            new("pristine", WordClass.Adjective, new[] { "quality", "excellent" }, "pristine"),
            new("rugged", WordClass.Adjective, new[] { "quality", "strong" }, "rugged"),
            new("sleek", WordClass.Adjective, new[] { "quality", "design" }, "sleek"),
            new("massive", WordClass.Adjective, new[] { "quality", "size" }, "massive"),
            new("compact", WordClass.Adjective, new[] { "quality", "size" }, "compact"),
            new("unstable", WordClass.Adjective, new[] { "quality", "danger" }, "unstable"),
            new("cutting-edge", WordClass.Adjective, new[] { "quality", "advanced" }, "cutting-edge"),

            // Sensory & Perception
            new("scanner", WordClass.Noun, new[] { "perception", "technology" }, "scanner"),
            new("visual", WordClass.Adjective, new[] { "perception", "sense" }, "visual"),
            new("sonic", WordClass.Adjective, new[] { "perception", "sense" }, "sonic"),
            new("infrared", WordClass.Adjective, new[] { "perception", "sense" }, "infrared"),
            new("spectrum", WordClass.Noun, new[] { "perception", "energy" }, "spectrum"),
            new("sensor", WordClass.Noun, new[] { "technology", "perception" }, "sensor"),

            // Celestial Bodies
            new("planet", WordClass.Noun, new[] { "space", "celestial" }, "planet"),
            new("moon", WordClass.Noun, new[] { "space", "celestial" }, "moon"),
            new("asteroid", WordClass.Noun, new[] { "space", "celestial" }, "asteroid"),
            new("comet", WordClass.Noun, new[] { "space", "celestial" }, "comet"),
            new("star", WordClass.Noun, new[] { "space", "star" }, "star"),
            new("sun", WordClass.Noun, new[] { "space", "star" }, "sun"),
            new("satellite", WordClass.Noun, new[] { "technology", "space" }, "satellite"),

            // Materials & Elements
            new("steel", WordClass.Noun, new[] { "material", "strength" }, "steel"),
            new("titanium", WordClass.Noun, new[] { "material", "strength" }, "titanium"),
            new("polymer", WordClass.Noun, new[] { "material", "technology" }, "polymer"),
            new("alloy", WordClass.Noun, new[] { "material", "craft" }, "alloy"),
            new("glass", WordClass.Noun, new[] { "material", "fragile" }, "glass"),

            // Movement & Navigation
            new("trajectory", WordClass.Noun, new[] { "movement", "path" }, "trajectory"),
            new("velocity", WordClass.Noun, new[] { "movement", "speed" }, "velocity"),
            new("acceleration", WordClass.Noun, new[] { "movement", "speed" }, "acceleration"),
            new("orbit", WordClass.Noun, new[] { "movement", "path" }, "orbit"),
            new("dock", WordClass.Verb, new[] { "movement", "action" }, "dock"),
            new("launch", WordClass.Verb, new[] { "movement", "action" }, "launch"),
            new("cruise", WordClass.Verb, new[] { "movement", "travel" }, "cruise"),
            new("maneuver", WordClass.Verb, new[] { "movement", "action" }, "maneuver"),

            // Knowledge & Study
            new("data", WordClass.Noun, new[] { "knowledge", "information" }, "data"),
            new("database", WordClass.Noun, new[] { "knowledge", "technology" }, "database"),
            new("research", WordClass.Noun, new[] { "knowledge", "study" }, "research"),
            new("analysis", WordClass.Noun, new[] { "knowledge", "study" }, "analysis"),
            new("theory", WordClass.Noun, new[] { "knowledge", "thought" }, "theory"),
            new("hypothesis", WordClass.Noun, new[] { "knowledge", "study" }, "hypothesis"),
            new("evidence", WordClass.Noun, new[] { "knowledge", "proof" }, "evidence"),
            new("archive", WordClass.Noun, new[] { "knowledge", "storage" }, "archive"),
            new("analyze", WordClass.Verb, new[] { "knowledge", "action" }, "analyze"),
            new("study", WordClass.Verb, new[] { "knowledge", "action" }, "study"),
            new("catalog", WordClass.Verb, new[] { "knowledge", "organization" }, "catalog"),

            // Emotion & Humanity
            new("hope", WordClass.Noun, new[] { "emotion", "positive" }, "hope"),
            new("despair", WordClass.Noun, new[] { "emotion", "negative" }, "despair"),
            new("courage", WordClass.Noun, new[] { "emotion", "virtue" }, "courage"),
            new("fear", WordClass.Noun, new[] { "emotion", "negative" }, "fear"),
            new("wonder", WordClass.Noun, new[] { "emotion", "positive" }, "wonder"),
            new("curiosity", WordClass.Noun, new[] { "emotion", "action" }, "curiosity"),
            new("determination", WordClass.Noun, new[] { "emotion", "virtue" }, "determination"),
            new("unity", WordClass.Noun, new[] { "emotion", "bond" }, "unity"),

            // Verbs of Creation & Destruction
            new("construct", WordClass.Verb, new[] { "creation", "action" }, "construct"),
            new("dismantle", WordClass.Verb, new[] { "destruction", "action" }, "dismantle"),
            new("assemble", WordClass.Verb, new[] { "creation", "action" }, "assemble"),
            new("decompose", WordClass.Verb, new[] { "destruction", "action" }, "decompose"),
            new("annihilate", WordClass.Verb, new[] { "destruction", "power" }, "annihilate"),
            new("fabricate", WordClass.Verb, new[] { "creation", "technology" }, "fabricate"),

            // Failure & Success
            new("triumph", WordClass.Noun, new[] { "success", "victory" }, "triumph"),
            new("victory", WordClass.Noun, new[] { "success", "power" }, "victory"),
            new("defeat", WordClass.Noun, new[] { "failure", "loss" }, "defeat"),
            new("collapse", WordClass.Noun, new[] { "failure", "destruction" }, "collapse"),
            new("salvage", WordClass.Verb, new[] { "success", "action" }, "salvage"),
            new("fail", WordClass.Verb, new[] { "failure", "action" }, "fail"),

            // Communication & Interface
            new("terminal", WordClass.Noun, new[] { "technology", "communication" }, "terminal"),
            new("interface", WordClass.Noun, new[] { "technology", "connection" }, "interface"),
            new("display", WordClass.Noun, new[] { "technology", "communication" }, "display"),
            new("screen", WordClass.Noun, new[] { "technology", "communication" }, "screen"),
            new("console", WordClass.Noun, new[] { "technology", "control" }, "console"),
            new("control", WordClass.Noun, new[] { "technology", "power" }, "control"),
            new("command", WordClass.Noun, new[] { "authority", "action" }, "command"),
            new("execute", WordClass.Verb, new[] { "technology", "action" }, "execute"),
            new("transmit", WordClass.Verb, new[] { "technology", "communication" }, "transmit"),
            new("receive", WordClass.Verb, new[] { "technology", "communication" }, "receive"),

            // Infection & Corruption Extended
            new("pathogen", WordClass.Noun, new[] { "danger", "biology" }, "pathogen"),
            new("toxin", WordClass.Noun, new[] { "danger", "biology" }, "toxin"),
            new("poison", WordClass.Noun, new[] { "danger", "biology" }, "poison"),
            new("contaminate", WordClass.Verb, new[] { "danger", "action" }, "contaminate"),
            new("degrade", WordClass.Verb, new[] { "danger", "destruction" }, "degrade"),

            // Engineering & Construction
            new("weld", WordClass.Verb, new[] { "technology", "creation" }, "weld"),
            new("bolt", WordClass.Noun, new[] { "technology", "connection" }, "bolt"),
            new("seal", WordClass.Verb, new[] { "technology", "protection" }, "seal"),
            new("pressure", WordClass.Noun, new[] { "physics", "force" }, "pressure"),
            new("stress", WordClass.Noun, new[] { "physics", "force" }, "stress"),
            new("weight", WordClass.Noun, new[] { "physics", "force" }, "weight"),
            new("mass", WordClass.Noun, new[] { "physics", "property" }, "mass"),
            new("density", WordClass.Noun, new[] { "physics", "property" }, "density"),

            // Environment & Atmosphere
            new("atmosphere", WordClass.Noun, new[] { "environment", "air" }, "atmosphere"),
            new("gravity", WordClass.Noun, new[] { "physics", "force" }, "gravity"),
            new("radiation", WordClass.Noun, new[] { "energy", "danger" }, "radiation"),
            new("thermal", WordClass.Adjective, new[] { "energy", "heat" }, "thermal"),
            new("vacuum", WordClass.Noun, new[] { "environment", "void" }, "vacuum"),
            new("atmospheric", WordClass.Adjective, new[] { "environment", "air" }, "atmospheric"),
            new("breathable", WordClass.Adjective, new[] { "environment", "life" }, "breathable"),

            // Biological Functions
            new("reproduce", WordClass.Verb, new[] { "biology", "creation" }, "reproduce"),
            new("consume", WordClass.Verb, new[] { "biology", "action" }, "consume"),
            new("secrete", WordClass.Verb, new[] { "biology", "action" }, "secrete"),
            new("digest", WordClass.Verb, new[] { "biology", "action" }, "digest"),
            new("absorb", WordClass.Verb, new[] { "biology", "action" }, "absorb"),
            new("excrete", WordClass.Verb, new[] { "biology", "action" }, "excrete"),
            new("metabolize", WordClass.Verb, new[] { "biology", "process" }, "metabolize"),

            // Consciousness Extended
            new("memory", WordClass.Noun, new[] { "mind", "knowledge" }, "memory"),
            new("instinct", WordClass.Noun, new[] { "mind", "behavior" }, "instinct"),
            new("emotion", WordClass.Noun, new[] { "mind", "feeling" }, "emotion"),
            new("logic", WordClass.Noun, new[] { "mind", "thought" }, "logic"),
            new("reason", WordClass.Verb, new[] { "mind", "action" }, "reason"),
            new("examine", WordClass.Verb, new[] { "mind", "action" }, "examine"),
            new("interpret", WordClass.Verb, new[] { "mind", "action" }, "interpret"),
            new("understand", WordClass.Verb, new[] { "mind", "knowledge" }, "understand"),

            // Signals & Frequencies
            new("frequency", WordClass.Noun, new[] { "communication", "technology" }, "frequency"),
            new("wavelength", WordClass.Noun, new[] { "communication", "energy" }, "wavelength"),
            new("amplitude", WordClass.Noun, new[] { "communication", "measurement" }, "amplitude"),
            new("modulate", WordClass.Verb, new[] { "communication", "action" }, "modulate"),
            new("encode", WordClass.Verb, new[] { "communication", "action" }, "encode"),
            new("decode", WordClass.Verb, new[] { "communication", "action" }, "decode"),
            new("encrypt", WordClass.Verb, new[] { "communication", "security" }, "encrypt"),
            new("decrypt", WordClass.Verb, new[] { "communication", "security" }, "decrypt"),

            // Navigation & Location
            new("position", WordClass.Noun, new[] { "movement", "location" }, "position"),
            new("bearing", WordClass.Noun, new[] { "movement", "direction" }, "bearing"),
            new("heading", WordClass.Noun, new[] { "movement", "direction" }, "heading"),
            new("approach", WordClass.Verb, new[] { "movement", "action" }, "approach"),
            new("retreat", WordClass.Verb, new[] { "movement", "action" }, "retreat"),

            // Degradation & Decay
            new("corrosion", WordClass.Noun, new[] { "danger", "decay" }, "corrosion"),
            new("oxidation", WordClass.Noun, new[] { "chemistry", "decay" }, "oxidation"),
            new("erosion", WordClass.Noun, new[] { "danger", "decay" }, "erosion"),
            new("weathering", WordClass.Noun, new[] { "environment", "decay" }, "weathering"),
            new("entropy", WordClass.Noun, new[] { "physics", "disorder" }, "entropy"),

            // Data & Processing
            new("byte", WordClass.Noun, new[] { "technology", "information" }, "byte"),
            new("bit", WordClass.Noun, new[] { "technology", "information" }, "bit"),
            new("buffer", WordClass.Noun, new[] { "technology", "storage" }, "buffer"),
            new("cache", WordClass.Noun, new[] { "technology", "storage" }, "cache"),
            new("storage", WordClass.Noun, new[] { "technology", "storage" }, "storage"),
            new("processor", WordClass.Noun, new[] { "technology", "computation" }, "processor"),
            new("compute", WordClass.Verb, new[] { "technology", "action" }, "compute"),
            new("process", WordClass.Verb, new[] { "technology", "action" }, "process"),

            // Scale & Magnitude
            new("microscopic", WordClass.Adjective, new[] { "scale", "small" }, "microscopic"),
            new("miniature", WordClass.Adjective, new[] { "scale", "small" }, "miniature"),
            new("vast", WordClass.Adjective, new[] { "scale", "large" }, "vast"),
            new("immense", WordClass.Adjective, new[] { "scale", "large" }, "immense"),
            new("colossal", WordClass.Adjective, new[] { "scale", "large" }, "colossal"),

            // Force & Power Extended
            new("thrust", WordClass.Noun, new[] { "physics", "force" }, "thrust"),
            new("momentum", WordClass.Noun, new[] { "physics", "motion" }, "momentum"),
            new("inertia", WordClass.Noun, new[] { "physics", "motion" }, "inertia"),
            new("friction", WordClass.Noun, new[] { "physics", "resistance" }, "friction"),
            new("drag", WordClass.Noun, new[] { "physics", "resistance" }, "drag"),
            new("resistance", WordClass.Noun, new[] { "physics", "opposition" }, "resistance"),

            // Precision & Accuracy
            new("calibrated", WordClass.Adjective, new[] { "quality", "precise" }, "calibrated"),
            new("precise", WordClass.Adjective, new[] { "quality", "exact" }, "precise"),
            new("accurate", WordClass.Adjective, new[] { "quality", "exact" }, "accurate"),
            new("tolerant", WordClass.Adjective, new[] { "quality", "flexible" }, "tolerant"),
            new("aberration", WordClass.Noun, new[] { "error", "deviation" }, "aberration"),
            new("variance", WordClass.Noun, new[] { "error", "deviation" }, "variance"),

            // Visibility & Concealment
            new("opaque", WordClass.Adjective, new[] { "perception", "hidden" }, "opaque"),
            new("transparent", WordClass.Adjective, new[] { "perception", "visible" }, "transparent"),
            new("translucent", WordClass.Adjective, new[] { "perception", "visible" }, "translucent"),
            new("obscured", WordClass.Adjective, new[] { "perception", "hidden" }, "obscured"),
            new("luminous", WordClass.Adjective, new[] { "perception", "light" }, "luminous"),
            new("bioluminescent", WordClass.Adjective, new[] { "biology", "light" }, "bioluminescent"),

            // Threat Assessment
            new("threat", WordClass.Noun, new[] { "danger", "risk" }, "threat"),
            new("hazard", WordClass.Noun, new[] { "danger", "risk" }, "hazard"),
            new("peril", WordClass.Noun, new[] { "danger", "risk" }, "peril"),
            new("catastrophe", WordClass.Noun, new[] { "danger", "disaster" }, "catastrophe"),
            new("disaster", WordClass.Noun, new[] { "danger", "loss" }, "disaster"),

            // Regeneration & Recovery
            new("regenerate", WordClass.Verb, new[] { "biology", "recovery" }, "regenerate"),
            new("recover", WordClass.Verb, new[] { "recovery", "action" }, "recover"),
            new("repair", WordClass.Verb, new[] { "recovery", "action" }, "repair"),
            new("restore", WordClass.Verb, new[] { "recovery", "action" }, "restore"),
            new("reconstruct", WordClass.Verb, new[] { "recovery", "action" }, "reconstruct"),

            // Harmony & Resonance
            new("harmony", WordClass.Noun, new[] { "balance", "order" }, "harmony"),
            new("resonance", WordClass.Noun, new[] { "physics", "vibration" }, "resonance"),
            new("vibration", WordClass.Noun, new[] { "physics", "motion" }, "vibration"),
            new("oscillate", WordClass.Verb, new[] { "physics", "motion" }, "oscillate"),
            new("synchronize", WordClass.Verb, new[] { "technology", "action" }, "synchronize"),

            // Transformation & Change
            new("transform", WordClass.Verb, new[] { "change", "action" }, "transform"),
            new("transmute", WordClass.Verb, new[] { "change", "action" }, "transmute"),
            new("alter", WordClass.Verb, new[] { "change", "action" }, "alter"),
            new("shift", WordClass.Verb, new[] { "change", "action" }, "shift"),
            new("transition", WordClass.Noun, new[] { "change", "movement" }, "transition"),

            // Bird People (flight + sound tags)
            new("sky", WordClass.Noun, new[] { "flight", "nature" }, "sky"),
            new("wing", WordClass.Noun, new[] { "flight", "body" }, "wing"),
            new("feather", WordClass.Noun, new[] { "flight", "adornment" }, "feather"),
            new("soar", WordClass.Verb, new[] { "flight", "action" }, "soar"),
            new("glide", WordClass.Verb, new[] { "flight", "action" }, "glide"),
            new("song", WordClass.Noun, new[] { "flight", "sound" }, "song"),
            new("chirp", WordClass.Noun, new[] { "flight", "sound" }, "chirp"),
            new("sing", WordClass.Verb, new[] { "flight", "action" }, "sing"),
            new("keen", WordClass.Verb, new[] { "flight", "sound" }, "keen"),
            new("swift", WordClass.Adjective, new[] { "flight", "speed" }, "swift"),
            new("aerial", WordClass.Adjective, new[] { "flight", "nature" }, "aerial"),
            new("wind", WordClass.Noun, new[] { "flight", "element" }, "wind"),
            new("updraft", WordClass.Noun, new[] { "flight", "movement" }, "updraft"),
            new("descent", WordClass.Noun, new[] { "flight", "movement" }, "descent"),
            new("howl", WordClass.Noun, new[] { "nature", "sound" }, "howl"),

            // Rock People (geology tag)
            new("stone", WordClass.Noun, new[] { "geology", "material" }, "stone"),
            new("granite", WordClass.Noun, new[] { "geology", "material" }, "granite"),
            new("bedrock", WordClass.Noun, new[] { "geology", "foundation" }, "bedrock"),
            new("crystal", WordClass.Noun, new[] { "geology", "structure" }, "crystal"),
            new("tremor", WordClass.Noun, new[] { "geology", "force" }, "tremor"),
            new("quake", WordClass.Noun, new[] { "geology", "force" }, "quake"),
            new("fracture", WordClass.Noun, new[] { "geology", "break" }, "fracture"),
            new("solid", WordClass.Adjective, new[] { "geology", "property" }, "solid"),
            new("unyielding", WordClass.Adjective, new[] { "geology", "strength" }, "unyielding"),
            new("ancient", WordClass.Adjective, new[] { "geology", "age" }, "ancient"),
            new("grind", WordClass.Verb, new[] { "geology", "action" }, "grind"),
            new("shatter", WordClass.Verb, new[] { "geology", "action" }, "shatter"),
            new("forge", WordClass.Verb, new[] { "geology", "action" }, "forge"),

            // Mycoids (biology/growth tags)
            new("mycelium", WordClass.Noun, new[] { "biology", "growth" }, "mycelium"),
            new("spore", WordClass.Noun, new[] { "biology", "reproduction" }, "spore"),
            new("fungus", WordClass.Noun, new[] { "biology", "organism" }, "fungus"),
            new("bloom", WordClass.Noun, new[] { "biology", "growth" }, "bloom"),
            new("growth", WordClass.Noun, new[] { "biology", "change" }, "growth"),
            new("proliferate", WordClass.Verb, new[] { "biology", "growth" }, "proliferate"),
            new("spread", WordClass.Verb, new[] { "biology", "movement" }, "spread"),
            new("root", WordClass.Noun, new[] { "biology", "structure" }, "root"),
            new("substrate", WordClass.Noun, new[] { "biology", "environment" }, "substrate"),

            // Mammalian Alien (nature tags)
            new("beast", WordClass.Noun, new[] { "nature", "creature" }, "beast"),
            new("fang", WordClass.Noun, new[] { "nature", "weapon" }, "fang"),
            new("claw", WordClass.Noun, new[] { "nature", "weapon" }, "claw"),
            new("fur", WordClass.Noun, new[] { "nature", "covering" }, "fur"),
            new("mane", WordClass.Noun, new[] { "nature", "adornment" }, "mane"),
            new("roar", WordClass.Verb, new[] { "nature", "action" }, "roar"),
            new("prowl", WordClass.Verb, new[] { "nature", "action" }, "prowl"),
            new("hunt", WordClass.Verb, new[] { "nature", "action" }, "hunt"),
            new("predator-noun", WordClass.Noun, new[] { "nature", "creature" }, "predator"),
            new("wild", WordClass.Adjective, new[] { "nature", "untamed" }, "wild"),
            new("feral", WordClass.Adjective, new[] { "nature", "untamed" }, "feral"),
            new("primal", WordClass.Adjective, new[] { "nature", "ancient" }, "primal"),
            new("savage", WordClass.Adjective, new[] { "nature", "fierce" }, "savage"),

            // Plantoid (nature/growth tags)
            new("plant", WordClass.Noun, new[] { "nature", "life" }, "plant"),
            new("stem", WordClass.Noun, new[] { "nature", "structure" }, "stem"),
            new("leaf", WordClass.Noun, new[] { "nature", "structure" }, "leaf"),
            new("bloom-verb", WordClass.Verb, new[] { "nature", "growth" }, "bloom"),
            new("blossom", WordClass.Noun, new[] { "nature", "flower" }, "blossom"),
            new("photosynthesis", WordClass.Noun, new[] { "nature", "process" }, "photosynthesis"),
            new("chlorophyll", WordClass.Noun, new[] { "nature", "substance" }, "chlorophyll"),
            new("cultivate", WordClass.Verb, new[] { "nature", "action" }, "cultivate"),
            new("flourish", WordClass.Verb, new[] { "nature", "growth" }, "flourish"),
            new("wither", WordClass.Verb, new[] { "nature", "decay" }, "wither"),
            new("seed", WordClass.Noun, new[] { "nature", "life" }, "seed"),
            new("sprout", WordClass.Verb, new[] { "nature", "growth" }, "sprout"),
            new("vinelike", WordClass.Adjective, new[] { "nature", "growth" }, "vinelike"),
            new("verdant", WordClass.Adjective, new[] { "nature", "life" }, "verdant"),

            // Reptilian (predator tag)
            new("scale", WordClass.Noun, new[] { "predator", "biology" }, "scale"),
            new("slither", WordClass.Verb, new[] { "predator", "movement" }, "slither"),
            new("venom", WordClass.Noun, new[] { "predator", "biology" }, "venom"),
            new("ambush", WordClass.Verb, new[] { "predator", "action" }, "ambush"),
            new("patient", WordClass.Adjective, new[] { "predator", "behavior" }, "patient"),
            new("cold-blooded", WordClass.Adjective, new[] { "predator", "cold" }, "cold-blooded"),
            new("cold", WordClass.Adjective, new[] { "predator", "cold" }, "cold"),
            new("serpentine", WordClass.Adjective, new[] { "predator", "form" }, "serpentine"),
            new("coil", WordClass.Noun, new[] { "predator", "form" }, "coil"),
            new("apex", WordClass.Noun, new[] { "predator", "rank" }, "apex"),

            // Hivemind (hivemind tag)
            new("consensus", WordClass.Noun, new[] { "hivemind", "collective" }, "consensus"),
            new("broadcast", WordClass.Verb, new[] { "hivemind", "communication" }, "broadcast"),
            new("distributed", WordClass.Adjective, new[] { "hivemind", "structure" }, "distributed"),
            new("parallel", WordClass.Adjective, new[] { "hivemind", "structure" }, "parallel"),
            new("index", WordClass.Noun, new[] { "hivemind", "identity" }, "index"),
            new("shard", WordClass.Noun, new[] { "hivemind", "identity" }, "shard"),
            new("replicate", WordClass.Verb, new[] { "hivemind", "action" }, "replicate"),
            new("fork", WordClass.Verb, new[] { "hivemind", "action" }, "fork"),
            new("join", WordClass.Verb, new[] { "hivemind", "action" }, "join"),

            // Grayfolk (grayfolk tag)
            new("observer", WordClass.Noun, new[] { "grayfolk", "role" }, "observer"),
            new("psionic", WordClass.Adjective, new[] { "grayfolk", "mind" }, "psionic"),
            new("telepathy", WordClass.Noun, new[] { "grayfolk", "mind" }, "telepathy"),
            new("watcher", WordClass.Noun, new[] { "grayfolk", "role" }, "watcher"),
            new("enigma", WordClass.Noun, new[] { "grayfolk", "mystery" }, "enigma"),
            new("silence", WordClass.Noun, new[] { "grayfolk", "state" }, "silence"),
            new("orb", WordClass.Noun, new[] { "grayfolk", "form" }, "orb"),
            new("lens", WordClass.Noun, new[] { "grayfolk", "form" }, "lens"),
            new("unfathomable", WordClass.Adjective, new[] { "grayfolk", "mystery" }, "unfathomable"),
            new("still", WordClass.Adjective, new[] { "grayfolk", "state" }, "still"),
        });
}
