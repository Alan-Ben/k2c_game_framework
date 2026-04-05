using System.Collections.Generic;

namespace GOE
{
    public partial class GRefdataCoreMgr
    {
        private void _initAnecdote()
        {
            if (anecdoteEventRefCore?.refList != null)
            {
                foreach (AnecdoteEventRefObj eventRef in anecdoteEventRefCore.refList)
                {
                    if (eventRef == null)
                        continue;
                    
                    eventRef.event_type_ref = anecdoteEventRewardRefCore.getRef(eventRef.id);
                    if (eventRef.event_type_ref != null)
                        continue;

                    eventRef.event_type_ref = anecdoteEventChoiceRefCore.getRef(eventRef.id);
                    if (eventRef.event_type_ref != null)
                        continue;

                    eventRef.event_type_ref = anecdoteEventEarningsRefCore.getRef(eventRef.id);
                    if (eventRef.event_type_ref != null)
                        continue;
                }
            }
            
            if (anecdoteEventChoiceRefCore?.refList != null && anecdoteEventChoiceOptionRefCore != null)
            {
                foreach (AnecdoteEventChoiceRefObj eventChoiceRef in anecdoteEventChoiceRefCore.refList)
                {
                    if (eventChoiceRef?.option_id_list == null)
                        continue;

                    eventChoiceRef.option_ref_list = new List<AnecdoteEventChoiceOptionRefObj>(eventChoiceRef.option_id_list.Count);
                    foreach (long optionId in eventChoiceRef.option_id_list)
                    {
                        AnecdoteEventChoiceOptionRefObj optionRef = anecdoteEventChoiceOptionRefCore.getRef(optionId);
                        if (optionRef != null)
                            eventChoiceRef.option_ref_list.Add(optionRef);
                    }
                }
            }
        } 
    }
}