INSERT INTO workflow.workflow_version (id, workflow_id, version, data_status, file_name, create_user, update_user, create_date, update_date, properties, description)
VALUES (DEFAULT, 1, '1.0', 'SUCCESS','test1.py', 'administrator', 'administrator', DEFAULT, DEFAULT, NULL, NULL),
       (DEFAULT, 1, '1.0', 'SUCCESS','test2.py', 'administrator', 'administrator', DEFAULT, DEFAULT, NULL, NULL),
       (DEFAULT, 2, '1.0', 'SUCCESS','test1.py', 'administrator', 'administrator', DEFAULT, DEFAULT, NULL, NULL),
       (DEFAULT, 2, '1.0', 'SUCCESS','test2.py', 'administrator', 'administrator', DEFAULT, DEFAULT, NULL, NULL);