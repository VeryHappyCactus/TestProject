
\qecho '----------------------Start Tables.sql---------------------\n'
\i Scripts/01_Tables.sql
\qecho '----------------------End Tables.sql---------------------\n'

\qecho '----------------------Start Data.sql---------------------\n'
\i Scripts/02_Data.sql
\qecho '----------------------End Data.sql---------------------\n'

\qecho '----------------------Start FN1.sql---------------------\n'
\i Scripts/03_FN1.sql 
\qecho '----------------------End FN1.sql---------------------\n'

\qecho '----------------------Start FN2.sql---------------------\n'
\i Scripts/04_FN2.sql 
\qecho '----------------------End FN2.sql---------------------\n'

\qecho '----------------------Start SP.sql---------------------\n'
\i Scripts/05_SP.sql 
\qecho '----------------------End SP.sql---------------------\n'
