INSERT INTO workflow.job (id, batch_job_id, worker_id, type, workflow_version_id, status, register_date, start_date, end_date, parameter)
VALUES ('TestJob1', 'TestBatchJob2', 'TestWorker2', 'PIPELINE', 1, 'SUCCESS', DEFAULT, DEFAULT, DEFAULT, ''),
       ('TestJob2', 'TestBatchJob2', 'TestWorker2', 'PIPELINE', 1, 'SUCCESS', DEFAULT, DEFAULT, DEFAULT, ''),
       ('TestJob3', 'TestBatchJob2', 'TestWorker2', 'PIPELINE', 1, 'SUCCESS', DEFAULT, DEFAULT, DEFAULT, ''),
       ('TestJob4', 'TestBatchJob2', 'TestWorker2', 'PIPELINE', 1, 'SUCCESS', DEFAULT, DEFAULT, DEFAULT, ''),
       ('TestJob5', 'TestBatchJob2', 'TestWorker2', 'PIPELINE', 1, 'SUCCESS', DEFAULT, DEFAULT, DEFAULT, ''),
       ('TestJob6', 'TestBatchJob3', 'TestWorker', 'RECIPE', 1, 'SUCCESS', DEFAULT, DEFAULT, DEFAULT, ''),
       ('TestJob7', 'TestBatchJob3', 'TestWorker', 'RECIPE', 1, 'SUCCESS', DEFAULT, DEFAULT, DEFAULT, ''),
       ('TestJob8', 'TestBatchJob3', 'TestWorker', 'RECIPE', 1, 'FAIL', DEFAULT, DEFAULT, DEFAULT, ''),
       ('TestJob9', 'TestBatchJob3', 'TestWorker', 'RECIPE', 1, 'SUCCESS', DEFAULT, DEFAULT, DEFAULT, ''),
       ('TestJob10', 'TestBatchJob3', 'TestWorker', 'RECIPE', 1, 'FAIL', DEFAULT, DEFAULT, DEFAULT, '');
       