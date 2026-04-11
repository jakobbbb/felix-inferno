#!/usr/bin/env python3

import os
import csv
import sys
from typing import List
import subprocess


class LineInfo:
    line_id: str
    text: str
    node: str

    @staticmethod
    def read_file(path: str) -> List[LineInfo]:
        lines = []
        with open(path, "r") as f:
            reader = csv.reader(f)
            for row in reader:
                line = LineInfo()
                line.line_id = row[1]
                line.text = row[2]
                line.node = row[4]
                lines.append(line)
        return lines


class YarnRecorder:
    def __init__(self, csv_file: str) -> None:
        self.lines = LineInfo.read_file(csv_file)
        pass

    def print_nodes(self) -> None:
        nodes = set([line.node for line in self.lines])
        print("Nodes:")
        print("\n".join(nodes))

    def record_lines(self, node: str) -> None:
        for line in self.lines:
            if line.node == node:
                self.record_line(line)

    def record_line(self, line: LineInfo) -> None:
        print(line.text)
        self.write_state_html(False, line.text)

        output_dir = "../Audio/"
        line_id_underscore = line.line_id.replace(":", "_")
        take = 0
        for f in os.listdir(output_dir):
            if line_id_underscore in f:
                take += 1

        input("Ready to record?  Press enter")
        filename = output_dir + line_id_underscore + f"_{take:02d}" + ".wav"
        self.rec = subprocess.Popen(
            ["arecord", "-vv", "--format=cd", filename]
        )
        self.write_state_html(True, line.text)
        input("Done?  Press enter")

        self.write_state_html(False, line.text)
        self.rec.terminate()
        subprocess.run("clear")

    def write_state_html(self, recording: bool, line: str):
        html = (
            '<meta http-equiv="refresh" content="1">'
            "<style>.rec { color: red; }</style>"
            f"<h1 class='{'rec' if recording else ''}'"
            f"style='font-size: 2em'>{line}</h1>"
        )
        with open("/tmp/record.html", "w+") as f:
            f.write(html)


if __name__ == "__main__":
    y = YarnRecorder(sys.argv[1])
    if len(sys.argv) == 3:
        y.record_lines(sys.argv[2])
    else:
        y.print_nodes()
