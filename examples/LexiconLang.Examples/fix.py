import os
import glob

target_dir = "/Users/ianlintner/Projects/lexiconlang_net/examples/LexiconLang.Examples"

for file in glob.glob(os.path.join(target_dir, "*.cs")):
    with open(file, "r", encoding="utf-8") as f:
        content = f.read()
    
    # Simple fix: the extra template code added by `dotnet new` is the only thing defining `namespace lexiconlang_net`
    if "namespace lexiconlang_net" in content:
        idx = content.find("namespace lexiconlang_net")
        # Go back slightly to remove the duplicate usings too
        # Look for the last `using` before `namespace lexiconlang_net`? No, the dummy template uses `using System.Collections.Generic;` 
        # which isn't in our new code at the top. Let's just split by `using System;\nusing System.Collections.Generic;`
        idx2 = content.find("using System;\nusing System.Collections.Generic;\nusing System.IO;\nusing System.Linq;\nusing System.Threading.Tasks;\n\nnamespace lexiconlang_net")
        if idx2 != -1:
            clean_content = content[:idx2].strip() + "\n"
            with open(file, "w", encoding="utf-8") as f:
                f.write(clean_content)
        else:
            # Maybe slightly different newlines (CRLF vs LF)
            # Find the index of the first rogue using:
            rogue_idx = content.rfind("using System;\nusing System.Collections.Generic;")
            if rogue_idx != -1:
                clean_content = content[:rogue_idx].strip() + "\n"
                with open(file, "w", encoding="utf-8") as f:
                    f.write(clean_content)
            else:
                idx3 = content.rfind("using System;")
                # Careful not to snip the top!
        print(f"Fixed {file}")