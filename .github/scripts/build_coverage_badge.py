import json
import pathlib
import xml.etree.ElementTree as ET

badge_path = pathlib.Path("coverage-badge.json")

# Parse Cobertura XML
files = sorted(pathlib.Path("TestResults").rglob("*.cobertura.xml"))
if not files:
    raise SystemExit("Coverage file not found")

root = ET.parse(files[-1]).getroot()
pct = round(float(root.attrib["line-rate"]) * 100, 1)

# Pick badge color
if pct >= 90:
    color = "brightgreen"
elif pct >= 75:
    color = "green"
elif pct >= 60:
    color = "yellowgreen"
elif pct >= 50:
    color = "yellow"
else:
    color = "red"

# Check previous badge
if badge_path.exists():
    old = json.loads(badge_path.read_text())
    old_pct = float(old["message"].rstrip("%"))

    if abs(pct - old_pct) <= 0.5:
        print(
            f"Coverage changed from {old_pct}% to {pct}% "
            "(≤0.5%), skipping badge update."
        )
        raise SystemExit(0)

badge = {
    "schemaVersion": 1,
    "label": "coverage",
    "message": f"{pct:.1f}%",
    "color": color,
}

badge_path.write_text(json.dumps(badge), encoding="utf-8")
print(f"Updated badge to {pct:.1f}%")
