import xml.etree.ElementTree as ET
import csv

# XML 파일을 파싱합니다.
path = '../Data/renjunet_v10_20231029.xml'
tree = ET.parse(path)
root = tree.getroot()

# CSV 파일을 열고 헤더를 작성합니다.
with open('../Data/gameData.csv', 'w', newline='') as csv_file:
    csv_writer = csv.writer(csv_file)
    csv_writer.writerow(["id", "bresult", "moves"])

    # game 태그를 찾아서 데이터를 추출합니다.
    for game in root.findall(".//game"):
        rule = game.get("rule")
        if rule == "1":
            id = game.get("id")
            bresult = game.get("bresult")
            moves = game.find("move").text
            if moves is None:
                continue

            # CSV 파일에 데이터를 작성합니다.
            csv_writer.writerow([id, bresult, moves])
