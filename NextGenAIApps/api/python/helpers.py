import pandas as pd
import ahocorasick
from io import StringIO

def extract_abbr(text: str) -> str:
    StringData = StringIO("""abbreviation	description
    \"\"\"\"	INCH
    #	NUMBER
    #	POUNDS
    &	AND
    1/16	SIXTEENTH
    1/2	HALF
    1/3	THIRD
    1/4	QUARTER
    1/5	FIFTH
    1/8	EIGHTH
    16F	16-FUNCTION
    1G	ONE~GANG
    1G	ONE-GANG
    1G	ONE GANG
    CNTLVR	CANTILEVER
    CNTNTL	CONTINENTAL
    CNTNUS	CONTINUOUS
    CNTR	CENTER
    """)
    model = ahocorasick.Automaton()
    data = pd.read_csv(StringData, sep="\t").to_records(index=False)
    for abbr, description in data:
        model.add_word(description, (description, abbr))
    model.make_automaton()

    abbr_seen = set()
    result = ""
    for idx, (desc, abbr) in model.iter(text):
        stop_idx = idx + 1
        start_idx = stop_idx - len(desc)
        prev_idx = max(start_idx - 1, 0)
        next_idx = min(stop_idx + 1, len(text))
        if (not text[prev_idx:start_idx].isalnum()) & (not text[stop_idx:next_idx].isalnum()):
            if abbr not in abbr_seen:
                result += f"{desc} -> {abbr}"
                result += "\n"
                abbr_seen.add(abbr)
    return result.strip()
