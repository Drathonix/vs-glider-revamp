using System;
using System.Collections.Generic;
using System.Text;

namespace GliderRevamp.Util;

public class Harmixin
{
    public static IEnumerable<CodeInstruction> Splice(IEnumerable<CodeInstruction> instructions, 
        List<CodeInstruction> prefix,
        IEnumerable<CodeInstruction> newInstructions,
        int toRemove=0)
    {
        List<CodeInstruction> result = [];
        int n = 0;
        foreach (var inst in instructions)
        {
            result.Add(inst);
            if (n > -1 && prefix[n].opcode.Equals(inst.opcode) && prefix[n].operand.Equals(inst.operand))
            {
                n++;
                if(n == prefix.Count)
                {
                    result.RemoveRange(result.Count - toRemove,toRemove);
                    result.AddRange(newInstructions);
                    n = -1;
                }
            }
            else
            {
                n = 0;
            }
        }
        return result;
    }
}
