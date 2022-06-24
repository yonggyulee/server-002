INSERT INTO workflow.batch_job (id, title, workflow_type, total_count, status, register_date, start_date, end_date, register_user, properties, description)
VALUES ('TestBatchJob', 'TEST 1', 'RECIPE', 5,'INPROGRESS', DEFAULT, DEFAULT, DEFAULT, 'administrator', NULL, NULL),
       ('TestBatchJob2', 'TEST 2', 'PIPELINE', 5,'SUCCESS', DEFAULT, DEFAULT, DEFAULT, 'administrator', NULL, NULL),
       ('TestBatchJob3', 'TEST 3', 'RECIPE', 5,'FAIL', DEFAULT, DEFAULT, DEFAULT, 'administrator', NULL, NULL);