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
            if (n > -1) {
                if (prefix[n].opcode.Equals(inst.opcode) && prefix[n].operand.Equals(inst.operand))
                {
                    n++;
                    if (n == prefix.Count)
                    {
                        result.RemoveRange(result.Count - toRemove, toRemove);
                        result.AddRange(newInstructions);
                        n = -1;
                    }
                }
                else
                {
                    n = 0;
                }
            }
        }
        return result;
    }
    public static IEnumerable<CodeInstruction> RelocateAfter(IEnumerable<CodeInstruction> instructions,
        List<CodeInstruction> prefix,
        List<CodeInstruction> destinationPrefix,
        int toRemove = 0)
    {
        List<CodeInstruction> result = [];
        List<CodeInstruction> cutting;
        int n = 0;
        int phase = 0;
        foreach (var inst in instructions)
        {
            result.Add(inst);
            if (phase == 0) {
                if (prefix[n].opcode.Equals(inst.opcode) && prefix[n].operand.Equals(inst.operand))
                {
                    n++;
                    if (n == prefix.Count)
                    {
                        cutting = result.GetRange(result.Count - toRemove, toRemove);
                        result.RemoveRange(result.Count - toRemove, toRemove);
                        phase = 1;
                    }
                }
                else
                {
                    n = 0;
                }
            } else if(phase == 1)
            {
                if(destinationPrefix[n].opcode.Equals(inst.opcode) && destinationPrefix[n].operand.Equals(inst.operand))
                {
                    n++;
                    if (n == prefix.Count)
                    {
                        cutting = result.GetRange(result.Count - toRemove, toRemove);
                        result.AddRange(cutting);
                        phase = 2;
                    }
                }
                else
                {
                    n = 0;
                }
            }
        }
        return result;
    }
}
