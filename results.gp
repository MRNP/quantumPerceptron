set key right
plot "data.csv" using 1:($3==-1?$2:1/0) pt 7 lc rgb "red" title "classe 1", \
"data.csv" using 1:($3==1?$2:1/0) pt 7 lc rgb "blue" title "classe 2"
replot "output.txt" using 1:2 with lines linecolor rgb "green" linewidth 2 title "qPerceptron"
