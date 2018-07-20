set key right
plot "data.csv" using 1:($3==-1?$2:1/0) pt 7 lc rgb "red" title "classe 1", \
"data.csv" using 1:($3==1?$2:1/0) pt 7 lc rgb "blue" title "classe 2"
